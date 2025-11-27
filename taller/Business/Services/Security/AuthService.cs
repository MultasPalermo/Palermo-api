using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Mensajeria;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Data.Interfaces.IDataImplement.Security;
using Data.Interfaces.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.Auth;
using Entity.DTOs.Default.Auth.RestPasword;
using Entity.DTOs.Default.Me;
using Entity.DTOs.Default.ModelSecurityDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Telegram.Bot.Passport;
using Utilities.Custom;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUserMeRepository _userMeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRolUserRepository _rolUserRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        //private readonly IServiceEmail _emailService;
        private readonly IPasswordResetCodeRepository _passwordResetRepo;
        private readonly IMemoryCache _cache;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IVerificationService _verificationService;
        private readonly EncriptePassword _encrypter;
        private readonly EmailBackgroundQueue _queue;
        private readonly IServiceEmail _emailService;
        public AuthService(
            IUserRepository userData,
            ILogger<AuthService> logger,
            IRolUserRepository rolUserData,
            IMapper mapper,
            IServiceEmail emailService,
            IPasswordResetCodeRepository passwordResetRepo,
            IUserMeRepository userMeRepository,
            IMemoryCache memoryCache,
            IPasswordHasher<User> passwordHasher,
            IVerificationService verificationService,
            EncriptePassword encrypter,
            EmailBackgroundQueue emailQueue)
        {
            _userRepository = userData;
            _logger = logger;
            _rolUserRepository = rolUserData;
            _mapper = mapper;
           // _emailService = emailService;
            _passwordResetRepo = passwordResetRepo;
            _userMeRepository = userMeRepository;
            _cache = memoryCache;
            _passwordHasher = passwordHasher;
            _verificationService = verificationService;
            _encrypter = encrypter;
            _queue = emailQueue;
            _emailService = emailService;
        }

        private static string MeKey(int userId) => $"me:{userId}";

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            // 1. verificar código
            var isValidCode = _verificationService.ValidateCode(dto.email, dto.verificationCode, "verification");
            if (!isValidCode)
                throw new BusinessException("código de verificación incorrecto o expirado.");

            // 2. verificar si ya existe
            var existing = await _userRepository.FindEmail(dto.email);
            if (existing != null)
                throw new BusinessException("el correo ya está registrado.");

            // 3. mapear
            var person = _mapper.Map<Person>(dto);
            var user = _mapper.Map<User>(dto);

            // 🔥 aplicar hash identity (NO tu encrypter)
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.password);

            user.Person = person;

            // logs
            _logger.LogInformation("registrando user: {@user}", user);
            _logger.LogInformation("registrando persona: {@person}", person);

            // 4. crear
            await _userRepository.CreateAsync(user);

            if (user.id <= 0)
                throw new BusinessException("no se pudo crear el usuario.");

            // 5. asignar rol
            await _rolUserRepository.AsignateUserRTo(user);

            // 6. obtener creado
            var createdUser = await _userRepository.GetByIdAsync(user.id)
                                   ?? throw new BusinessException("error interno al obtener usuario.");

            InvalidateUserCache(user.id);

            return _mapper.Map<UserDto>(createdUser);
        }
        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _userRepository.FindEmail(email);
            if (user == null)
                throw new ValidationException("correo no registrado");

            // enviar código de recuperación
            await _verificationService.SendVerificationPasswordAsync(email);
        }

        public async Task ResetPasswordAsync(ConfirmResetDto dto)
        {
            var user = await _userRepository.FindEmail(dto.email)
                       ?? throw new ValidationException("usuario no encontrado");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.newPassword);

            await _userRepository.UpdateAsync(user);

            InvalidateUserCache(user.id);

            _cache.Remove(dto.email); // eliminar token/código usado
        }




        public async Task<UserMeDto> BuildUserContextAsync(int userId)
        {
            var cacheKey = MeKey(userId);

            if (_cache.TryGetValue(cacheKey, out UserMeDto cached))
                return cached;

            var user = await _userMeRepository.GetUserWithPersonAsync(userId)
                       ?? throw new BusinessException("Usuario no encontrado");

            var userRoles = await _userMeRepository.GetUserRolesWithPermissionsAsync(userId);

            var roles = userRoles
                .Where(ur => ur.active && !ur.is_deleted)
                .Select(ur => ur.Rol)
                .Where(r => r.active && !r.is_deleted)
                .DistinctBy(r => r.id)
                .ToList();

            var roleNames = roles.Select(r => r.name)
                                 .Where(n => !string.IsNullOrWhiteSpace(n))
                                 .Distinct()
                                 .ToList();

            var formPermissions = new Dictionary<int, HashSet<string>>();
            var permissions = new HashSet<string>();

            foreach (var role in roles)
            {
                foreach (var rfp in role.rol_form_permission)
                {
                    var pName = rfp.Permission?.name;
                    if (string.IsNullOrWhiteSpace(pName)) continue;

                    permissions.Add(pName);

                    if (!formPermissions.ContainsKey(rfp.FormId))
                        formPermissions[rfp.FormId] = new();

                    formPermissions[rfp.FormId].Add(pName);
                }
            }

            var formIds = formPermissions.Keys.ToList();
            var formsWithModules = await _userMeRepository.GetFormsWithModulesByIdsAsync(formIds);

            var modules = formsWithModules
                .Where(f => f.FormModules.Any())
                .GroupBy(f => f.FormModules.First().module)
                .Select(g =>
                {
                    var module = _mapper.Map<MenuModuleDto>(g.Key);
                    module.forms = g.Select(form =>
                    {
                        var dto = _mapper.Map<FormMeDto>(form);
                        dto.Permissions = formPermissions[form.id];
                        return dto;
                    }).ToList();

                    return module;
                }).ToList();

            var me = new UserMeDto
            {
                id = user.id,
                fullName = $"{user.Person.firstName} {user.Person.lastName}",
                firstName = user.Person.firstName,
                lastName = user.Person.lastName,
                email = user.email,
                roles = roleNames,
                permissions = permissions.ToList(),
                Menu = modules
            };

            _cache.Set(cacheKey, me, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return me;
        }


        public void InvalidateUserCache(int userId) => _cache.Remove(MeKey(userId));
    }
}



