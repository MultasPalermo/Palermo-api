using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{

    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class DocumentInfractionController : BaseController<DocumentInfractionDto, DocumentInfractionSelectDto, IDocumentInfractionServices>
    {
        public DocumentInfractionController(IDocumentInfractionServices services, ILogger<DocumentInfractionController> logger) : base(services, logger)
        {
        }
        protected override Task<IEnumerable<DocumentInfractionSelectDto>> GetAllAsync(GetAllType getAllType) => _service.GetAllAsync(getAllType);
        protected override Task<DocumentInfractionSelectDto?> GetByIdAsync(int id) => _service.GetByIdAsync(id);
        protected override Task AddAsync(DocumentInfractionDto dto) => _service.CreateAsync(dto);
        protected override Task<bool> UpdateAsync(int id, DocumentInfractionDto dto) => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType) => _service.DeleteAsync(id, deleteType);


        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);
    }
}
