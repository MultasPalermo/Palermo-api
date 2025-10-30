using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Helpers.Business.Business.Helpers.Validation;
using Helpers.Initialize;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class RolUserService
        : BusinessBasic<RolUserDto, RolUserSelectDto, RolUser>, IRolUserService
    {
        private readonly IRolUserRepository _rolUserRepository;
        private readonly ILogger<RolUserService> _logger;

        public RolUserService(
            IRolUserRepository data,
            IMapper mapper,
            ILogger<RolUserService> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            _rolUserRepository = data;
            _logger = logger;
        }

        // =============================================
        // Métodos obligatorios del BusinessBasic
        // =============================================

        public override async Task<IEnumerable<RolUserSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_rolUserRepository, getAllType);
                var entities = await strategy.GetAll(_rolUserRepository);
                return _mapper.Map<IEnumerable<RolUserSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los registros de RolUser.", ex);
            }
        }

        public override async Task<RolUserSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El RolUser con ID {id} no existe.");

                var entity = await _rolUserRepository.GetByIdAsync(id);
                return _mapper.Map<RolUserSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el RolUser con ID {id}.", ex);
            }
        }

        public override async Task<RolUserDto> CreateAsync(RolUserDto dto)
        {
            try
            {
                await ValidateForeignKeysAsync(dto);
                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el RolUser.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(RolUserDto dto)
        {
            try
            {
                await ValidateForeignKeysAsync(dto);
                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el RolUser.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. El RolUser con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el RolUser con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El RolUser con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el RolUser con ID {id}.", ex);
            }
        }

        // =============================================
        // Métodos adicionales de IRolUserService
        // =============================================

        public async Task<RolUserDto> AsignateUserRTo(User user)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(user, "El usuario no puede ser nulo.");

                var entity = await _rolUserRepository.AsignateUserRTo(user);
                entity.InitializeLogicalState();

                return _mapper.Map<RolUserDto>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al asignar el rol al usuario.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetAllRolUser(int idUser)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(idUser, "El ID de usuario debe ser mayor que cero.");
            return await _rolUserRepository.GetJoinRolesAsync(idUser);
        }

        public async Task<RolUserSelectDto?> GetByIdJoin(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _rolUserRepository.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<RolUserSelectDto>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        // =============================================
        // Helper interno
        // =============================================
        private async Task ValidateForeignKeysAsync(RolUserDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

            if (!await ExistsAsync(dto.userId))
                throw new BusinessException($"El usuario con ID {dto.userId} no existe.");

            if (!await ExistsAsync(dto.rolId))
                throw new BusinessException($"El rol con ID {dto.rolId} no existe.");
        }
    }
}
