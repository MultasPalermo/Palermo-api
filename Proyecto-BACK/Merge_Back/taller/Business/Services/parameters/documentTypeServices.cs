using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    public class documentTypeServices : BusinessBasic<documentTypeDto,documentTypeSelectDto,documentType>,IdocumentTypeServices
    {
        private readonly ILogger<documentTypeServices> _logger;

        protected readonly IData<documentType> Data;

        public documentTypeServices(IData<documentType> data, IMapper mapper, ILogger<documentTypeServices> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }
    }
}
