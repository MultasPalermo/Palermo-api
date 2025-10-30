//using Entity.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

//namespace Entity.Infrastructure.Contexts
//{
//    public class AuditDbContext : DbContext
//    {
//        private readonly IConfiguration _configuration;
//        public AuditDbContext(DbContextOptions<AuditDbContext> options, IConfiguration configuration) : base(options)
//        {
//            _configuration = configuration;  
//        }

//        // DbSets para posgrest
//        public DbSet<audit> audit {  get; set; }
//    }
//}
