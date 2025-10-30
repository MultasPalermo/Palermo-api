using Business.Interfaces.BusinessBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Select.EntitiesSelectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface IInstallmentScheduleServices : IBusiness<InstallmentScheduleDto, InstallmentScheduleSelectDto>
    {

    }
}
