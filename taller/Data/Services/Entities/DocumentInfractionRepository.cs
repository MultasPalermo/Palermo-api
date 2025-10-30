using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Entities
{
    public class DocumentInfractionRepository : DataGeneric<DocumentInfraction>,IDocumentInfractionRepository
    {
        public DocumentInfractionRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public override async Task<IEnumerable<DocumentInfraction>> GetAllAsync()
        {
            return await _dbSet
                        .Include(d => d.paymentAgreement)
                        .Include(d => d.inspectoraReport)
                        .Where(d => d.is_deleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<DocumentInfraction>> GetDeletes()
        {
            return await _dbSet
                        .Include(d => d.paymentAgreement)
                        .Include(d => d.inspectoraReport)
                        .Where(d => d.is_deleted == true)
                        .ToListAsync();
        }

        public override async Task<DocumentInfraction?> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(d => d.paymentAgreement)
                      .Include(d => d.inspectoraReport)
                      .Where(d => d.id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
