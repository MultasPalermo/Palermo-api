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
    public class FormService : BusinessBasic<FormDto, FormSelectDto, Form>, IFormService
    {
        private readonly ILogger<FormService> _logger;
        //protected override IData<Form> Data => _unitOfWork.Forms;
        protected readonly IData<Form> Data;
        public FormService(IData<Form> data, IMapper mapper, ILogger<FormService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }


        //protected override void ValidateDto(FormDto dto)
        //{
        //    if (dto == null)
        //    {
        //        throw new ValidationException("El objeto Form no puede ser nulo");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.name))
        //    {
        //        _logger.LogWarning("Se intentó crear/actualizar una Form con Name vacío");
        //        throw new ValidationException("name", "El Name de la Form es obligatorio");
        //    }
        //}

        //protected override async Task ValidateIdAsync(int id)
        //{
        //    var entity = await Data.GetByIdAsync(id);
        //    if (entity == null)
        //    {
        //        _logger.LogWarning($"Se intentó operar un ID inválido: {id}");
        //        throw new EntityNotFoundException($"No se encontró un Form con el ID {id}");
        //    }
        //}
    }
}
