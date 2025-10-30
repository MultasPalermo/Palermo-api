
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Stategy.Interfaz
{
    public interface IDeleteStrategy<TEntity> where TEntity : class
    {
        Task<bool> DeleteAsync(IDataDelete<TEntity> data, int id);
    }

    //public interface IDeleteStrategy<T> where T : class
    //{
    //    Task<bool> DeleteAsync(T entity, DbContext context);
    //}

}
