using AutoMapper;
using Business.Strategy.StrategyDeletes.Implement;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Helpers.Business.Business.Helpers.Validation;
using Helpers.Initialize;
using Utilities.Exceptions;
using FluentValidation;
using FVValidationException = FluentValidation.ValidationException;
using UValidationException = Utilities.Exceptions.ValidationException;
using System.Runtime.ExceptionServices;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore; // para re-lanzar conservando stack

namespace Business.Repository
{
    public class BusinessBasic<TDto, TDtoGet, TEntity>
        : ABaseModelBusiness<TDto, TDtoGet, TEntity> where TEntity : BaseModel
    {
        protected readonly IMapper _mapper;
        protected readonly IData<TEntity> Data;
        protected readonly ApplicationDbContext? _context;

        public BusinessBasic(IData<TEntity> data, IMapper mapper, ApplicationDbContext? context = null)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context; // puede ser null si no lo necesitan
        }


        public override async Task<IEnumerable<TDtoGet>> GetAllAsync()
        {
            try
            {
                var entities = await Data.GetAllAsync();
                return _mapper.Map<IEnumerable<TDtoGet>>(entities);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<IEnumerable<TDtoGet>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet<TEntity>(Data, getAllType);
                var entities = await strategy.GetAll(Data);
                return _mapper.Map<IEnumerable<TDtoGet>>(entities);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al obtener registros con estrategia {getAllType}.", ex);
            }
        }

        public override async Task<TDtoGet?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await Data.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<TDtoGet>(entity);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<TDto> CreateAsync(TDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = _mapper.Map<TEntity>(dto);
                entity.InitializeLogicalState();

                var created = await Data.CreateAsync(entity);
                return _mapper.Map<TDto>(created);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException("Error al crear el registro.", ex);
            }
        }

        public override async Task<bool> UpdateAsync(TDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = _mapper.Map<TEntity>(dto);
                entity.InitializeLogicalState();
                return await Data.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al actualizar el registro.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
                return await Data.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al eliminar el registro con ID {id}.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id, DeleteType deleteType)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var strategy = DeleteStrategyFactory.GetStrategy<TEntity>(Data, deleteType);
                return await strategy.DeleteAsync(id, Data);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al eliminar registro (ID: {id}) con estrategia {deleteType}.", ex);
            }
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
                return await Data.RestoreAsync(id);
            }
            catch (Exception ex)
            {
                RethrowIfKnown(ex);
                throw new BusinessException($"Error al restaurar el registro con ID {id}.", ex);
            }
        }

        public override async Task<bool> ExistsAsync(int id)
        {
            BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
            return await _context.Set<TEntity>().AnyAsync(e => e.id == id);
        }



        /// <summary>
        /// Si es BusinessException, FluentValidation.ValidationException o Utilities.ValidationException, relanza sin envolver.
        /// </summary>
        private static void RethrowIfKnown(Exception ex)
        {
            if (ex is BusinessException ||
                ex is FVValidationException ||
                ex is UValidationException)
            {
                // Re-throw preservando el stack original (mejor que "throw ex;")
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

    }
}
