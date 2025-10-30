using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Enums;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Select.EntitiesSelectDto;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TypeInfractionController
        : BaseController<TypeInfractionDto, TypeInfractionSelectDto, ITypeInfractionServices>
    {
        public TypeInfractionController(ITypeInfractionServices service, ILogger<TypeInfractionController> logger)
           : base(service, logger) { }

        // Obtener todos los registros
        protected override Task<IEnumerable<TypeInfractionSelectDto>> GetAllAsync(GetAllType getAllType)
            => _service.GetAllAsync(getAllType);

        // Obtener registro por ID
        protected override Task<TypeInfractionSelectDto?> GetByIdAsync(int id)
            => _service.GetByIdAsync(id);

        // Crear nuevo registro
        protected override Task AddAsync(TypeInfractionDto dto)
            => _service.CreateAsync(dto);

        // Actualizar registro existente
        protected override Task<bool> UpdateAsync(int id, TypeInfractionDto dto)
            => _service.UpdateAsync(dto);

        // Eliminar registro
        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType)
            => _service.DeleteAsync(id, deleteType);

        // Restaurar lógico
        protected override Task<bool> RestaureAsync(int id)
            => _service.RestoreLogical(id);
    }
}
