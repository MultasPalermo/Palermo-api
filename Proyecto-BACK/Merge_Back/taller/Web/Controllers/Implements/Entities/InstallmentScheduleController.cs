using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Enums;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Select.EntitiesSelectDto;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InstallmentScheduleController : BaseController<InstallmentScheduleDto, InstallmentScheduleSelectDto, IInstallmentScheduleServices>
    {
        public InstallmentScheduleController(IInstallmentScheduleServices service, ILogger<InstallmentScheduleController> logger)
            : base(service, logger) { }

        protected override Task<IEnumerable<InstallmentScheduleSelectDto>> GetAllAsync(GetAllType getAllType)
            => _service.GetAllAsync(getAllType);

        protected override Task<InstallmentScheduleSelectDto?> GetByIdAsync(int id)
            => _service.GetByIdAsync(id);

        protected override Task AddAsync(InstallmentScheduleDto dto)
            => _service.CreateAsync(dto);

        protected override Task<bool> UpdateAsync(int id, InstallmentScheduleDto dto)
            => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType)
            => _service.DeleteAsync(id, deleteType);

        protected override Task<bool> RestaureAsync(int id)
            => _service.RestoreLogical(id);
    }
}
