using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services.Entities
{
    public class UserNotificationService : BusinessBasic<UserNotificationDto, UserNotificationSelectDto, UserNotification>, IUserNotificationService
    {
        private readonly ILogger<UserNotificationService> _logger;
        private readonly IUserNotificationRepository _repository;

        public UserNotificationService(IUserNotificationRepository repository, IMapper mapper, ILogger<UserNotificationService> logger)
            : base(repository, mapper)
        {
            _logger = logger;
            _repository = repository;
        }

        public override async Task<IEnumerable<UserNotificationSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_repository, getAllType);
                var entities = await strategy.GetAll(_repository);
                return _mapper.Map<IEnumerable<UserNotificationSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todas las notificaciones.", ex);
            }
        }

        public override async Task<UserNotificationSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<UserNotificationSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la notificación con ID {id}.", ex);
            }
        }
    }
}
