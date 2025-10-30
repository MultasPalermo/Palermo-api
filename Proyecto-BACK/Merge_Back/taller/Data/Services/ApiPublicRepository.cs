//using Data.Interfaces;
//using Data.Repositoy;
//using Entity.Domain.Models;
//using Entity.Infrastructure.Contexts;

//namespace Data.Services
//{
//    public class ApiPublicRepository : DataGeneric<TouristicAttraction> ,IApiPublic<TouristicAttraction>
//    {
//        public ApiPublicRepository(ApplicationDbContext context) : base(context){}

//        public async Task AddRangeAsync(IEnumerable<TouristicAttraction> entities)
//        {
//            await _dbSet.AddRangeAsync(entities);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
