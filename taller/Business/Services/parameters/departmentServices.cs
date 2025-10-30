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
    public class departmentServices : BusinessBasic<departmentDto,departmentSelectDto,department>,IdepartmentServices
    {
        private readonly ILogger<departmentServices> _logger;

        protected readonly IData<department> Data;

        public departmentServices(IData<department> data, IMapper mapper, ILogger<departmentServices> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }
    }
}
