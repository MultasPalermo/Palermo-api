using Business.Interfaces.BusinessBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;

namespace Business.Repository
{
    public abstract class ABaseModelBusiness<TDto, TDtoGet, TEntity> : IBusiness<TDto, TDtoGet> where TEntity : BaseModel
    {
        public abstract Task<TDto> CreateAsync(TDto dto);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> DeleteAsync(int id, DeleteType deleteType);
        public abstract Task<IEnumerable<TDtoGet>> GetAllAsync();
        public abstract Task<IEnumerable<TDtoGet>> GetAllAsync(GetAllType g);
        public abstract Task<TDtoGet?> GetByIdAsync(int id);
        public abstract Task<bool> RestoreLogical(int id);
        public abstract Task<bool> UpdateAsync(TDto dto);

        public virtual Task<bool> ExistsAsync(int id)
        {
            // Implementación por defecto: devuelve false o lanza una excepción
            return Task.FromResult(false);
        }

    }
}
