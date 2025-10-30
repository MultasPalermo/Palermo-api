using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Helpers.Business.Business.Helpers.Validation;

namespace Business.Services.Entities
{
    public class TypePaymentServices
        : BusinessBasic<TypePaymentDto, TypePaymentSelectDto, TypePayment>, ITypePaymentServices
    {
        private readonly ILogger<TypePaymentServices> _logger;
        protected readonly IData<TypePayment> Data;

        public TypePaymentServices(
            IData<TypePayment> data,
            IMapper mapper,
            ILogger<TypePaymentServices> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            Data = data;
            _logger = logger;
        }

        public override async Task<TypePaymentDto> CreateAsync(TypePaymentDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

            // ✅ Validación de clave foránea
            if (!await ExistsAsync(dto.paymentAgreementId))
                throw new BusinessException($"El acuerdo de pago con ID {dto.paymentAgreementId} no existe.");

            return await base.CreateAsync(dto);
        }

        public override async Task<bool> UpdateAsync(TypePaymentDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

            if (!await ExistsAsync(dto.paymentAgreementId))
                throw new BusinessException($"El acuerdo de pago con ID {dto.paymentAgreementId} no existe.");

            return await base.UpdateAsync(dto);
        }

        public override async Task<TypePaymentSelectDto?> GetByIdAsync(int id)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

            if (!await ExistsAsync(id))
                throw new BusinessException($"El tipo de pago con ID {id} no existe.");

            var entity = await Data.GetByIdAsync(id);
            return _mapper.Map<TypePaymentSelectDto?>(entity);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

            if (!await ExistsAsync(id))
                throw new BusinessException($"No se puede eliminar. El tipo de pago con ID {id} no existe.");

            return await base.DeleteAsync(id);
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

            if (!await ExistsAsync(id))
                throw new BusinessException($"No se puede restaurar. El tipo de pago con ID {id} no existe.");

            return await base.RestoreLogical(id);
        }
    }
}
