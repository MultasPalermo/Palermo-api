using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Repository;
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.LoginDto.response.RegisterReponseDto;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Default.RegisterRequestDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Entity.Infrastructure.Contexts;           // <-- DbContext
using Helpers.Business.Business.Helpers.Validation;
using Helpers.Initialize;
using Microsoft.EntityFrameworkCore;            // <-- Transacciones/consultas EF
using Microsoft.Extensions.Logging;
using Utilities.Custom;

namespace Business.Services.Security
{
    public class UserService : BusinessBasic<UserDto, UserSelectDto, User>, IUserService
    {
        private readonly ApplicationDbContext _db;      
        private readonly IPersonRepository _people;     
        private readonly IUserRepository _dataUser;
        private readonly ILogger<UserService> _logger;
        private readonly EncriptePassword _utilities;
        private readonly IRolUserService _rolUserService;
        private readonly IServiceEmail _email;

        public UserService(
            IUserRepository data,
            IPersonRepository people,                    
            ApplicationDbContext db,                     
            ILogger<UserService> logger,
            EncriptePassword utilities,
            IMapper mapper,
            IRolUserService rolUserService,
            IServiceEmail email
        ) : base(data, mapper)
        {
            _dataUser = data;
            _people = people;                            
            _db = db;                                   
            _utilities = utilities;
            _logger = logger;
            _rolUserService = rolUserService;
            _email = email;
        }

        // ========= Registro (Person + User + Rol por defecto) =========
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            // 0) Validación de unicidad
            var existing = await _dataUser.FindEmail(dto.email);
            if (existing != null && !existing.is_deleted)
                throw new InvalidOperationException("Ya existe una cuenta con ese email.");

            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1) Crear Person
                var person = _mapper.Map<Person>(dto);
                person.InitializeLogicalState();
                var personCreated = await _people.CreateAsync(person);

                // 2) Crear User
                var user = _mapper.Map<User>(dto);
                user.PersonId = personCreated.id;
                user.InitializeLogicalState();

                // 2.1) Generar código de verificación
                var code = new Random().Next(100000, 999999).ToString();
                user.EmailVerified = false;
                user.EmailVerificationCode = code;
                user.EmailVerificationExpiresAt = DateTime.UtcNow.AddMinutes(15);

                var userCreated = await _dataUser.CreateAsync(user);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                // 3) Asignar rol por defecto (no bloqueante)
                _ = _rolUserService.AsignateUserRTo(userCreated);

                // 4) Enviar email con código (no bloquear respuesta)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var builder = new VerificacionEmailBuilder(
                            personCreated.firstName ?? "Usuario",
                            code
                        );

                        await _email.SendEmailAsyncVerificacion(dto.email, builder);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "No se pudo enviar código de verificación a {Email}", dto.email);
                    }
                });

                return new RegisterResponseDto
                {
                    IsSuccess = true,
                    Message = "Registro completado. Revisa tu correo para ingresar el código de verificación."
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // UserService.cs
        public async Task<bool> VerifyCodeAsync(string code)
        {
            code = (code ?? "").Trim();
            if (code.Length == 0) return false;

            var user = await _dataUser.FindByVerificationCodeAsync(code);
            if (user == null) return false;

            if (user.EmailVerified) return true;

            if (!user.EmailVerificationExpiresAt.HasValue || user.EmailVerificationExpiresAt.Value < DateTimeOffset.UtcNow)
                return false;

            user.EmailVerified = true;
            user.EmailVerifiedAt = DateTime.UtcNow;
            user.EmailVerificationCode = null;
            user.EmailVerificationExpiresAt = null;

            await _db.SaveChangesAsync();
            return true;
        }

        // ========= Crear usuario por Google (si no existe) =========
        public async Task<User> createUserGoogle(string email, string name)
        {
            var user = await _dataUser.FindEmail(email);
            if (user != null) return user;

            var newUser = new User
            {
                PasswordHash = null,
                email = email
            };

            await _dataUser.CreateAsync(newUser);
            return newUser;
        }

        // ========= Actualizar =========
        public async Task<bool> UpdateAsyncUser(UserDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");
            var entity = _mapper.Map<User>(dto);

            // Si decides permitir cambio de password en el DTO:
            // if (!string.IsNullOrWhiteSpace(dto.PasswordPlain))
            //     entity.PasswordHash = _utilities.EncripteSHA256(dto.PasswordPlain);

            return await _dataUser.UpdateAsync(entity);
        }


    }
}
