using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services.Entities
{
    public class InfractionService : BusinessBasic<InfractionDto, InfractionSelectDto, Infraction>, IInfractionService
    {
        private readonly ILogger<InfractionService> _logger;
        private readonly IInfractionRepository _repository;

        public InfractionService(IInfractionRepository repository, IMapper mapper, ILogger<InfractionService> logger)
            : base(repository, mapper)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<IEnumerable<InfractionSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_repository, getAllType);
                var entities = await strategy.GetAll(_repository);
                return _mapper.Map<IEnumerable<InfractionSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de infracción.", ex);
            }
        }

        public override async Task<InfractionSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<InfractionSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro de infracción con ID {id}.", ex);
            }
        }
    }

}
