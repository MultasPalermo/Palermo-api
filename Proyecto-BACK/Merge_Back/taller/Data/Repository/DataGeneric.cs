using Data.Interfaces.DataBasic;
using Data.Repository;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositoy
{
    public class DataGeneric<T> : ABaseModelData<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public DataGeneric(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }


        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            //if (typeof(ISupportLogicalDelete).IsAssignableFrom(typeof(T)))
            //{
            //}
                return await _dbSet
                    .Where(e => e.is_deleted == false)
                    .ToListAsync();

            //return await _dbSet.ToListAsync();
        }

        public override async Task<IEnumerable<T>> GetDeletes()
        {
            //if (typeof(ISupportLogicalDelete).IsAssignableFrom(typeof(T)))
            //{
            //}
                return await _dbSet
                    .Where(e => e.is_deleted == true)
                    .ToListAsync();

            //return new List<T>();
        }

        public override async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is BaseModel deletable && deletable.is_deleted)
                return null;

            return entity;
        }
        public override async Task<T> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;

        }
        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<bool> DeleteLogicAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not BaseModel deletable) return false;

            deletable.is_deleted = true;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }




        public override async Task<bool> RestoreAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not BaseModel deletable) return false;

            if (!deletable.is_deleted) return false;

            deletable.is_deleted = false;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        ////Metodo adicional 
        //public override async Task AddRangeAsync(IEnumerable<T> entities)
        //{
        //    await _dbSet.AddRangeAsync(entities);
        //    await _context.SaveChangesAsync();
        //}

    }
}

