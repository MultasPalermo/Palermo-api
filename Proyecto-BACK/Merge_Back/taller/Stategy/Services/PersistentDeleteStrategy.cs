using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stategy.Interfaz;

namespace Stategy.Services
{
    public class PersistentDeleteStrategy<TEntity> : IDeleteStrategy<TEntity> where TEntity : class
    {
        public async Task<bool> DeleteAsync(IDataDelete<TEntity> data, int id)
        {
            return await data.DeleteAsync(id);
        }
    }

    //public class PersistentDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    //{
    //    public async Task<bool> DeleteAsync(T entity, DbContext context)
    //    {
    //        context.Set<T>().Remove(entity);
    //        await context.SaveChangesAsync();
    //        return true;
    //    }
    //}

}
