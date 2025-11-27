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
using Utilities.Exceptions;

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

        public async Task<bool> MarkInstallmentAsPaidAsync(int installmentId)
        {
            if (installmentId <= 0)
                throw new BusinessException("El ID de la cuota es inválido.");

            var cuota = await _data.GetByIdAsync(installmentId);

            if (cuota == null)
                throw new BusinessException($"La cuota {installmentId} no existe.");

            if (cuota.IsPaid)
                return true; // Ya estaba pagada

            cuota.IsPaid = true;
            cuota.RemainingBalance = 0;

            await _data.UpdateAsync(cuota);

            return true;
        }

    }
}
