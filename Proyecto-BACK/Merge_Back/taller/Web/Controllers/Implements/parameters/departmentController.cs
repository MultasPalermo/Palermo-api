using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;
using Web.Controllers.Implements.Entities;

namespace Web.Controllers.Implements.parameters
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class departmentController : BaseController<departmentDto,departmentSelectDto, IdepartmentServices>
    {
        public departmentController(IdepartmentServices services, ILogger<departmentController> logger) : base(services, logger)
        {
        }
        protected override Task<IEnumerable<departmentSelectDto>> GetAllAsync(GetAllType getAllType) => _service.GetAllAsync(getAllType);
        protected override Task<departmentSelectDto?> GetByIdAsync(int id) => _service.GetByIdAsync(id);
        protected override Task AddAsync(departmentDto dto) => _service.CreateAsync(dto);
        protected override Task<bool> UpdateAsync(int id, departmentDto dto) => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType) => _service.DeleteAsync(id, deleteType);


        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);
    }
}
