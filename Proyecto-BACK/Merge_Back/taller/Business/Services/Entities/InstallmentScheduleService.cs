using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Select.EntitiesSelectDto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Entities
{
    public class InstallmentScheduleService : BusinessBasic<InstallmentScheduleDto,InstallmentScheduleSelectDto,InstallmentSchedule>,IInstallmentScheduleServices
    {
        private readonly IInstallmentScheduleRepository _data;
        protected readonly ILogger<InstallmentScheduleService> _logger;

        public InstallmentScheduleService(IInstallmentScheduleRepository data,IMapper mapper,ILogger<InstallmentScheduleService> logger) : base(data, mapper)
        {
            _data = data;
            _logger = logger;
        }
    }
}
