using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Repository;
using Business.Services.Entities;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;
using Microsoft.Extensions.Logging;

namespace Business.Services.parameters
{
    public class PaymentFrequencyServices : BusinessBasic<PaymentFrequencyDto,PaymentFrequencySelectDto,PaymentFrequency>,IPaymentFrequencyServices
    {
        private readonly ILogger<PaymentFrequencyServices> _logger;

        protected readonly IData<PaymentFrequency> Data;

        public PaymentFrequencyServices(IData<PaymentFrequency> data, IMapper mapper, ILogger<PaymentFrequencyServices> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }
    }
}
