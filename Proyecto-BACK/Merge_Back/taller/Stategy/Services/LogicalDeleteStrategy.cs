using Data.Interfaces;
using Entity.Interfaz;
using Microsoft.EntityFrameworkCore;
using Stategy.Interfaz;

namespace Stategy.Services
{
    public class LogicalDeleteStrategy<TEntity> : IDeleteStrategy<TEntity> where TEntity : class, ISupportLogicalDelete
    {
        public async Task<bool> DeleteAsync(IDataDelete<TEntity> data, int id)
        {
            return await data.DeleteLogicalAsync(id);
        }
    }

    //public class LogicalDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    //{
    //    public async Task<bool> DeleteAsync(T entity, DbContext context)
    //    {
    //        if (entity is not ISupportLogicalDelete logicalEntity)
    //            throw new InvalidOperationException($"La entidad {typeof(T).Name} no soporta borrado lógico.");

    //        logicalEntity.is_deleted = true;
    //        context.Entry(entity).State = EntityState.Modified;
    //        await context.SaveChangesAsync();
    //        return true;
    //    }
    //}


}