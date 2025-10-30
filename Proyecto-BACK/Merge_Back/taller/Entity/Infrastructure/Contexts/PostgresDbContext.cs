    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Entity.Data.Seeds.parameters;
    using Entity.DataInit.dataInitModelSecurity;
    using Entity.DataInit.dataInitParameters;
    using Entity.DataInit.EntitiesDataInit;
    using Entity.DataInit.parametersDataInit;
    using Entity.Domain.Interfaces;
    using Entity.Domain.Models.Implements.Entities;
    using Entity.relacionesModel.RelacionesEntities;
    using Entity.relacionesModel.RelacionesModelSecurity;
    using Entity.relacionesModel.RelacionesParameters;
using Entity.Domain.Models.Implements.ModelSecurity;

public class PostgresDbContext : DbContext
    {
        //private readonly IConfiguration _configuration;
        //private readonly IAuditService _auditService;
        //private readonly IHttpContextAccessor _http;

        public PostgresDbContext(
            DbContextOptions<PostgresDbContext> options
            //IConfiguration configuration,
            //IAuditService auditService,
            //IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            //_configuration = configuration;
            //_auditService = auditService;
            //_http = httpContextAccessor;
        }

        // =========================
        // === DBSET Model Security
        // =========================
        public DbSet<User> users { get; set; }
        public DbSet<Person> persons { get; set; }
        public DbSet<Rol> rols { get; set; }
        public DbSet<RolUser> rolUsers { get; set; }
        public DbSet<Form> forms { get; set; }
        public DbSet<Module> modules { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<RolFormPermission> rol_form_permissions { get; set; }
        public DbSet<FormModule> form_modules { get; set; }

        // =========================
        // === DBSET Model Entities
        // =========================
        public DbSet<Infraction> Infraction { get; set; }
        public DbSet<InspectoraReport> inspectoraReport { get; set; }
        public DbSet<ValueSmldv> valueSmldv { get; set; }
        public DbSet<UserNotification> userNotification { get; set; }
        public DbSet<DocumentInfraction> documenInfraction { get; set; }
        public DbSet<TypePayment> typePayment { get; set; }
        public DbSet<UserInfraction> userInfraction { get; set; }
        public DbSet<FineCalculationDetail> fineCalculationDetail { get; set; }
        public DbSet<PaymentAgreement> paymentAgreement { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // ============ CONFIGURACIONES ============
            // Parameters
            modelBuilder.ApplyConfiguration(new RelacionDocumentType());
            modelBuilder.ApplyConfiguration(new RelacionDepartment());
            modelBuilder.ApplyConfiguration(new RelacionMunicipality());
            modelBuilder.ApplyConfiguration(new RelacionPaymentFrequency());

            // Model / Security
            modelBuilder.ApplyConfiguration(new RelacionPerson());
            modelBuilder.ApplyConfiguration(new RelacionUser());
            modelBuilder.ApplyConfiguration(new RelacionesRol());
            modelBuilder.ApplyConfiguration(new RelacionModule());
            modelBuilder.ApplyConfiguration(new RelacionPermission());
            modelBuilder.ApplyConfiguration(new RelacionForm());
            modelBuilder.ApplyConfiguration(new RelacionRolUser());
            modelBuilder.ApplyConfiguration(new RelacionFormModule());
            modelBuilder.ApplyConfiguration(new RelacionRolFormPermission());

            // Entities
            modelBuilder.ApplyConfiguration(new RelacionesTypeInfraction());
            modelBuilder.ApplyConfiguration(new RelacionesInspectoraReport());
            modelBuilder.ApplyConfiguration(new RelacionesValueSmldv());
            modelBuilder.ApplyConfiguration(new RelacionesUserNotification());
            modelBuilder.ApplyConfiguration(new RelacionesDocumentInfraction());
            modelBuilder.ApplyConfiguration(new RelacionesTypePayment());
            modelBuilder.ApplyConfiguration(new RelacionesUserInfraction());
        modelBuilder.ApplyConfiguration(new RelacionesFineCalculationDetail());
            modelBuilder.ApplyConfiguration(new RelacionesPaymentAgreement());

            // ============ SEEDS ============
            modelBuilder.SeedDocumentType();
            modelBuilder.SeedDepartment();
            modelBuilder.seedMunicipality();
            modelBuilder.SeedPaymentFrequency();

            modelBuilder.seedPerson();
            modelBuilder.SeedUser();

            modelBuilder.SeedRol();
            modelBuilder.SeedModule();
            modelBuilder.SeedPermission();
            modelBuilder.SeedForm();
            modelBuilder.SeedRolUser();
            modelBuilder.SeedFormModule();
            modelBuilder.SeedRolFormPermission();

            modelBuilder.SeddInfraction();
            modelBuilder.SeedTypeInfraction();
            modelBuilder.SeetInspectoraReportData();
            modelBuilder.SeedValueSmldv();
            modelBuilder.SeedUserNotificacion();
            modelBuilder.SeedDocumentInfraction();
            modelBuilder.SeedTypePayment();
            modelBuilder.SeedUserInfraction();
            modelBuilder.SeedFineCalculationDetail();
            modelBuilder.SeedPaymentAgreement();
        }

        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        ChangeTracker.DetectChanges();
        //        _auditService.CaptureAsync(ChangeTracker).GetAwaiter().GetResult();
        //        return base.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error in SaveChanges: {ex.Message}");
        //        throw;
        //    }
        //}

        //public override async Task<int> SaveChangesAsync(
        //    bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        ChangeTracker.DetectChanges();
        //        await _auditService.CaptureAsync(ChangeTracker, cancellationToken);
        //        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error in SaveChangesAsync: {ex.Message}");
        //        throw;
        //    }
        //}
}

