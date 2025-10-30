using AutoMapper;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class PersonService
        : BusinessBasic<PersonDto, PersonSelectDto, Person>, IPersonService
    {
        private readonly ILogger<PersonService> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonService(
            IPersonRepository data,
            IMapper mapper,
            ILogger<PersonService> logger,
            Entity.Infrastructure.Contexts.ApplicationDbContext context
        ) : base(data, mapper, context)
        {
            _personRepository = data;
            _logger = logger;
        }

        public override async Task<IEnumerable<PersonSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_personRepository, getAllType);
                var entities = await strategy.GetAll(_personRepository);
                return _mapper.Map<IEnumerable<PersonSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<PersonSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"La persona con ID {id} no existe.");

                var entity = await _personRepository.GetByIdAsync(id);
                return _mapper.Map<PersonSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<PersonDto> CreateAsync(PersonDto dto)
        {
            try
            {

                if (!await ExistsAsync(dto.municipalityId))
                    throw new BusinessException($"El municipio con ID {dto.municipalityId} no existe.");

                return await base.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear la persona.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(PersonDto dto)
        {
            try
            {


                if (!await ExistsAsync(dto.municipalityId))
                    throw new BusinessException($"El municipio con ID {dto.municipalityId} no existe.");

                return await base.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la persona.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede eliminar. La persona con ID {id} no existe.");

                return await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar la persona con ID {id}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. La persona con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar la persona con ID {id}.", ex);
            }
        }

    }
}
