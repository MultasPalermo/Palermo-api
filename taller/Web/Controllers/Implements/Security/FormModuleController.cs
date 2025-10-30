using Business.Interfaces.IBusinessImplements;
using Business.Interfaces.IBusinessImplements.Security;
using Entity.Domain.Enums;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Security
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class FormModuleController : BaseController<FormModuleDto, FormModuleSelectDto, IFormModuleService>
    {
        public FormModuleController(IFormModuleService service, ILogger<FormModuleController> logger) : base(service, logger)
        {
        }

        protected override Task<IEnumerable<FormModuleSelectDto>> GetAllAsync(GetAllType getAllType)=> _service.GetAllAsync(getAllType);

        protected override Task<FormModuleSelectDto?> GetByIdAsync(int id) => _service.GetByIdAsync(id);

        protected override Task AddAsync(FormModuleDto dto) => _service.CreateAsync(dto);

        protected override Task<bool> UpdateAsync(int id, FormModuleDto dto) => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType) => _service.DeleteAsync(id, deleteType);

        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);

    }
}
