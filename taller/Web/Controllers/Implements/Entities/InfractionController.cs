using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InfractionController : BaseController<InfractionDto, InfractionSelectDto, IInfractionService>
    {
        public InfractionController(IInfractionService service, ILogger<InfractionController> logger)
            : base(service, logger) { }

        protected override Task<IEnumerable<InfractionSelectDto>> GetAllAsync(GetAllType getAllType) => _service.GetAllAsync(getAllType);
        protected override Task<InfractionSelectDto?> GetByIdAsync(int id) => _service.GetByIdAsync(id);
        protected override Task AddAsync(InfractionDto dto) => _service.CreateAsync(dto);
        protected override Task<bool> UpdateAsync(int id, InfractionDto dto) => _service.UpdateAsync(dto);
        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType) => _service.DeleteAsync(id, deleteType);
        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);


    }
}
