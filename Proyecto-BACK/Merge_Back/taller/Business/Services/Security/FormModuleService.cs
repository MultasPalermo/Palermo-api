using AutoMapper;
using Business.Interfaces.IBusinessImplements;
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
    public class FormModuleService
        : BusinessBasic<FormModuleDto, FormModuleSelectDto, FormModule>, IFormModuleService
    {
        private readonly IFormModuleRepository _formModuleRepository;
        private readonly ILogger<FormModuleService> _logger;

        public FormModuleService(
            IData<FormModule> data,
            IMapper mapper,
            IFormModuleRepository formModuleRepository,
            ILogger<FormModuleService> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            _formModuleRepository = formModuleRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<FormModuleSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_formModuleRepository, getAllType);
                var entities = await strategy.GetAll(_formModuleRepository);
                return _mapper.Map<IEnumerable<FormModuleSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<FormModuleSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El FormModule con ID {id} no existe.");

                var entity = await _formModuleRepository.GetByIdAsync(id);
                return _mapper.Map<FormModuleSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<FormModuleDto> CreateAsync(FormModuleDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                // ✅ Validación de claves foráneas
                if (!await ExistsAsync(dto.formid))
                    throw new BusinessException($"El formulario con ID {dto.formid} no existe.");

                if (!await ExistsAsync(dto.moduleid))
                    throw new BusinessException($"El módulo con ID {dto.moduleid} no existe.");

                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el FormModule.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(FormModuleDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                if (!await ExistsAsync(dto.formid))
                    throw new BusinessException($"El formulario con ID {dto.formid} no existe.");

                if (!await ExistsAsync(dto.moduleid))
                    throw new BusinessException($"El módulo con ID {dto.moduleid} no existe.");

                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el FormModule.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. El FormModule con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el FormModule con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El FormModule con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el FormModule con ID {id}.", ex);
            }
        }
    }
}
