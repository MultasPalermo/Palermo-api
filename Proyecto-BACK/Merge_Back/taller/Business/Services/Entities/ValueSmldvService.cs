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
    public class ValueSmldvService : BusinessBasic<ValueSmldvDto, ValueSmldvSelectDto, ValueSmldv>, IValueSmldvService
    {
        private readonly ILogger<ValueSmldvService> _logger;
        protected readonly IValueSmldvRepository _repository;

        public ValueSmldvService(IValueSmldvRepository repository, IMapper mapper, ILogger<ValueSmldvService> logger)
            : base(repository, mapper)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<IEnumerable<ValueSmldvSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_repository, getAllType);
                var entities = await strategy.GetAll(_repository);
                return _mapper.Map<IEnumerable<ValueSmldvSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de SMLDV.", ex);
            }
        }

        public override async Task<ValueSmldvSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor a cero.");

                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<ValueSmldvSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }
    }

}
