using AutoMapper;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.parameters;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.parameters
{
    public class municipalityServices
        : BusinessBasic<municipalityDto, municipalitySelectDto, municipality>, ImunicipalityServices
    {
        private readonly ILogger<municipalityServices> _logger;
        private readonly ImunicipalityRepository _municipalityRepository;

        public municipalityServices(
            ImunicipalityRepository municipalityRepository,
            IMapper mapper,
            ILogger<municipalityServices> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(municipalityRepository, mapper, context)
        {
            _municipalityRepository = municipalityRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<municipalitySelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_municipalityRepository, getAllType);
                var entities = await strategy.GetAll(_municipalityRepository);
                return _mapper.Map<IEnumerable<municipalitySelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<municipalitySelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El municipio con ID {id} no existe.");

                var entity = await _municipalityRepository.GetByIdAsync(id);
                return _mapper.Map<municipalitySelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<municipalityDto> CreateAsync(municipalityDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                // ✅ Validar que el departamento exista
                if (!await ExistsAsync(dto.departmentId))
                    throw new BusinessException($"El departamento con ID {dto.departmentId} no existe.");

                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el municipio.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(municipalityDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                if (!await ExistsAsync(dto.departmentId))
                    throw new BusinessException($"El departamento con ID {dto.departmentId} no existe.");

                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el municipio.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. El municipio con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el municipio con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El municipio con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el municipio con ID {id}.", ex);
            }
        }
    }
}
