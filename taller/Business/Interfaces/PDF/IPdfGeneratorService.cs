using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Select.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.PDF
{
    public interface IPdfGeneratorService
    {
         Task<byte[]> GeneratePdfAsync(UserInfractionSelectDto dto);

        Task<byte[]> GeneratePaymentAgreementPdfAsync(PaymentAgreementSelectDto dto);


    }
}
