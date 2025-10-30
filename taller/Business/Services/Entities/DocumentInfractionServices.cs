using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Entities
{
    public class DocumentInfractionServices
        : BusinessBasic<DocumentInfractionDto, DocumentInfractionSelectDto, DocumentInfraction>, IDocumentInfractionServices
    {
        private readonly ILogger<DocumentInfractionServices> _logger;
        private readonly IDocumentInfractionRepository _documentInfractionRepository;

        public DocumentInfractionServices(
            IDocumentInfractionRepository data,
            IMapper mapper,
            ILogger<DocumentInfractionServices> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            _documentInfractionRepository = data;
            _logger = logger;
        }

        public override async Task<IEnumerable<DocumentInfractionSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_documentInfractionRepository, getAllType);
                var entities = await strategy.GetAll(_documentInfractionRepository);
                return _mapper.Map<IEnumerable<DocumentInfractionSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<DocumentInfractionSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El documento de infracción con ID {id} no existe.");

                var entity = await _documentInfractionRepository.GetByIdAsync(id);
                return _mapper.Map<DocumentInfractionSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<DocumentInfractionDto> CreateAsync(DocumentInfractionDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                // ✅ Validación de claves foráneas
                if (!await ExistsAsync(dto.inspectoraReportId))
                    throw new BusinessException($"El reporte de inspectora con ID {dto.inspectoraReportId} no existe.");

                if (!await ExistsAsync(dto.PaymentAgreementId))
                    throw new BusinessException($"El acuerdo de pago con ID {dto.PaymentAgreementId} no existe.");

                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el documento de infracción.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(DocumentInfractionDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                if (!await ExistsAsync(dto.inspectoraReportId))
                    throw new BusinessException($"El reporte de inspectora con ID {dto.inspectoraReportId} no existe.");

                if (!await ExistsAsync(dto.PaymentAgreementId))
                    throw new BusinessException($"El acuerdo de pago con ID {dto.PaymentAgreementId} no existe.");

                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el documento de infracción.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. El documento de infracción con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el registro con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El documento de infracción con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el registro con ID {id}.", ex);
            }
        }
    }
}
