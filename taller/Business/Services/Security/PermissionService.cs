using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class PermissionService : BusinessBasic<PermissionDto, PermissionSelectDto, Permission>, IPermissionService
    {
        private readonly ILogger<PermissionService> _logger;
        //protected override IData<Permission> Data => _unitOfWork.Permissions;
        protected readonly IData<Permission> Data;
        public PermissionService(IData<Permission> data, IMapper mapper, ILogger<PermissionService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }


        //protected override void ValidateDto(PermissionDto dto)
        //{
        //    if (dto == null)
        //    {
        //        throw new ValidationException("El objeto Permission no puede ser nulo");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.name))
        //    {
        //        _logger.LogWarning("Se intentó crear/actualizar una Permission con Name vacío");
        //        throw new ValidationException("name", "El Name de la Permission es obligatorio");
        //    }
        //}

        //protected override async Task ValidateIdAsync(int id)
        //{
        //    var entity = await Data.GetByIdAsync(id);
        //    if (entity == null)
        //    {
        //        _logger.LogWarning($"Se intentó operar un ID inválido: {id}");
        //        throw new EntityNotFoundException($"No se encontró un Permission con el ID {id}");
        //    }
        //}
    }
}
