using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Entities
{
    public class UserNotificationRepository : DataGeneric<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(ApplicationDbContext context) : base(context)
        {
        }


        public override async Task<IEnumerable<UserNotification>> GetAllAsync()
        {
            return await _dbSet
            .Include(n => n.userInfraction)
            .Where(n => n.is_deleted == false)
            .ToListAsync();
        }


        public override async Task<IEnumerable<UserNotification>> GetDeletes()
        {
            return await _dbSet
            .Include(n => n.userInfraction)
            .Where(n => n.is_deleted == true)
            .ToListAsync();
        }


        public override async Task<UserNotification?> GetByIdAsync(int id)
        {
            return await _dbSet
            .Include(n => n.userInfraction)
            .Where(n => n.id == id)
            .FirstOrDefaultAsync();
        }
    }
}

