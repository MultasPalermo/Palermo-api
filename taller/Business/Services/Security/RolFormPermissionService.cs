using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class RolFormPermissionService
        : BusinessBasic<RolFormPermissionDto, RolFormPermissionSelectDto, RolFormPermission>, IRolFormPermissionService
    {
        private readonly IRolFormPermissionRepository _rolFormPermissionRepository;
        private readonly ILogger<RolFormPermissionService> _logger;

        public RolFormPermissionService(
            IData<RolFormPermission> data,
            IMapper mapper,
            IRolFormPermissionRepository rolFormPermissionRepository,
            ILogger<RolFormPermissionService> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            _rolFormPermissionRepository = rolFormPermissionRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<RolFormPermissionSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_rolFormPermissionRepository, getAllType);
                var entities = await strategy.GetAll(_rolFormPermissionRepository);
                return _mapper.Map<IEnumerable<RolFormPermissionSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<RolFormPermissionSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El RolFormPermission con ID {id} no existe.");

                var entity = await _rolFormPermissionRepository.GetByIdAsync(id);
                return _mapper.Map<RolFormPermissionSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<RolFormPermissionDto> CreateAsync(RolFormPermissionDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                // ✅ Validar que las claves foráneas existan
                if (!await ExistsAsync(dto.rolid))
                    throw new BusinessException($"El rol con ID {dto.rolid} no existe.");

                if (!await ExistsAsync(dto.formid))
                    throw new BusinessException($"El formulario con ID {dto.formid} no existe.");

                if (!await ExistsAsync(dto.permissionid))
                    throw new BusinessException($"El permiso con ID {dto.permissionid} no existe.");

                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el RolFormPermission.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(RolFormPermissionDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                if (!await ExistsAsync(dto.rolid))
                    throw new BusinessException($"El rol con ID {dto.rolid} no existe.");

                if (!await ExistsAsync(dto.formid))
                    throw new BusinessException($"El formulario con ID {dto.formid} no existe.");

                if (!await ExistsAsync(dto.permissionid))
                    throw new BusinessException($"El permiso con ID {dto.permissionid} no existe.");

                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el RolFormPermission.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. El RolFormPermission con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el RolFormPermission con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El RolFormPermission con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el RolFormPermission con ID {id}.", ex);
            }
        }
    }
}
