

using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Microsoft.Extensions.Logging;

namespace Business.Services.Security
{
    public class RolService : BusinessBasic<RolDto, RolSelectDto, Rol>, IRolService
    {

        private readonly ILogger<RolService> _logger;
        protected readonly IData<Rol> Data;

        public RolService(IData<Rol> data, IMapper mapper, ILogger<RolService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;  
        }


        //protected override void ValidateDto(RolDto dto)
        //{
        //    if (dto == null)
        //    {
        //        throw new ValidationException("El objeto Rol no puede ser nulo");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.name))
        //    {
        //        _logger.LogWarning("Se intentó crear/actualizar una Rol con Name vacío");
        //        throw new ValidationException("user_name", "El UserName de la Rol es obligatorio");
        //    }
        //}


        //protected override async Task ValidateIdAsync(int id)
        //{
        //    var entity = await Data.GetByIdAsync(id);
        //    if (entity == null)
        //    {
        //        _logger.LogWarning($"Se intentó operar un ID inválido: {id}");
        //        throw new EntityNotFoundException($"No se encontró un Rol con el ID {id}");
        //    }
        //}
    }
}
