using Entity.Data.Seeds.parameters;
using Entity.DataInit.dataInitModelSecurity;
using Entity.DataInit.dataInitParameters;
using Entity.DataInit.EntitiesDataInit;
using Entity.DataInit.parametersDataInit;
using Entity.Domain.Interfaces;
using Entity.Domain.Models;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Domain.Models.Implements.parameters;
using Entity.relacionesModel;
using Entity.relacionesModel.RelacionesEntities;
using Entity.relacionesModel.RelacionesModelSecurity;
using Entity.relacionesModel.RelacionesParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Entity.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
       // protected readonly IConfiguration _configuration;
        //private readonly IAuditService _auditService;
        //private readonly IHttpContextAccessor _http;
        //protected readonly IConfiguration _configuration;
        ////private readonly IAuditService _auditService;
        //private readonly IHttpContextAccessor _http;

        public ApplicationDbContext(
             DbContextOptions<ApplicationDbContext> options
            // IConfiguration configuration,
             //IAuditService auditService,
             //IHttpContextAccessor httpContextAccessor
             //IConfiguration configuration,
             //IAuditService auditService,
             //IHttpContextAccessor httpContextAccessor
         ) : base(options)
        {
           // _configuration = configuration;
            //_auditService = auditService;
            //_http = httpContextAccessor;
            //_configuration = configuration;
            ////_auditService = auditService;
            //_http = httpContextAccessor;
        }

        ///<summary>
        ///Implementación DBSet Model security
        ///</summary>
        public DbSet<User> users { get; set; }
        public DbSet<Person> persons { get; set; }
        public DbSet<Rol> rols { get; set; }
        public DbSet<RolUser> rolUsers { get; set; }
        public DbSet<Form> forms { get; set; }
        public DbSet<Domain.Models.Implements.ModelSecurity.Module> modules { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<RolFormPermission> rol_form_permissions { get; set; }
        public DbSet<FormModule> form_modules { get; set; }
        //public DbSet<TouristicAttraction> TouristicAttraction { get; set; }

        ///<summary>
        ///Implementación DBSet Model entities
        ///</summary>

        public DbSet<Infraction> Infraction { get; set; }
        public DbSet<InspectoraReport> inspectoraReport { get; set; }
        public DbSet<ValueSmldv> valueSmldv { get; set; }
        public DbSet<UserNotification> userNotification { get; set; }
        public DbSet<DocumentInfraction> documenInfraction { get; set; }
        public DbSet<TypePayment> typePayment { get; set; }
        public DbSet<UserInfraction> userInfraction { get; set; }
        public DbSet<FineCalculationDetail> fineCalculationDetail { get; set; }
        public DbSet<PaymentAgreement> paymentAgreement { get; set; }

        public DbSet<InstallmentSchedule> installmentSchedule { get; set; }

        //parametros
        public DbSet<AuthSession> AuthSessions { get; set; } = null!;
        public DbSet<RefreshToken> refreshTokens { get; set; }

        public DbSet<documentType> documentTypes { get; set; }
        public DbSet<department> Departments { get; set; }
        public DbSet<municipality> Municipality { get; set; }
        public DbSet<PaymentFrequency> paymentFrequency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          

            // ============ CONFIGURACIONES (IEntityTypeConfiguration<>) ============
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
            modelBuilder.ApplyConfiguration(new RelacionesInstallmentSchedule());

            modelBuilder.ApplyConfiguration(new AuthSessionConfig());

            // ============ SEEDS (ORDEN IMPORTA) ============

            // 1) Parameters primero (padres de varias FKs)
            modelBuilder.SeedDocumentType();
            modelBuilder.SeedDepartment();
            modelBuilder.seedMunicipality();
            modelBuilder.SeedPaymentFrequency();

            // 2) Model / Security
            //    Person depende de documentType y municipality (usa FKs sombra en tu seed actual)
            modelBuilder.seedPerson();

            //    User normalmente depende de Person (si tu seed de User referencia personId)
            modelBuilder.SeedUser();

            //    Catálogos y relaciones de seguridad
            modelBuilder.SeedRol();
            modelBuilder.SeedModule();
            modelBuilder.SeedPermission();
            modelBuilder.SeedForm();
            modelBuilder.SeedRolUser();
            modelBuilder.SeedFormModule();
            modelBuilder.SeedRolFormPermission();

            // 3) Entities (dependencias de negocio)
            //    Catálogos/maestros que otros usan:
            modelBuilder.SeddInfraction();
            modelBuilder.SeedTypeInfraction();
            modelBuilder.SeetInspectoraReportData();
            modelBuilder.SeedValueSmldv();
            modelBuilder.SeedUserNotificacion();
            modelBuilder.SeedDocumentInfraction();
            modelBuilder.SeedTypePayment();

            //    Entidades que dependen de lo anterior:
            //    UserInfraction suele depender de User/TypeInfraction/DocumentInfraction, por eso va después.
            modelBuilder.SeedUserInfraction();

            //    FineCalculationDetail a menudo depende de UserInfraction -> va después de SeedUserInfraction
            modelBuilder.SeedFineCalculationDetail();

            //    PaymentAgreement depende de UserInfraction y PaymentFrequency (y sus tablas puente de M:N
            //    con DocumentInfraction/TypePayment deben existir antes). Por eso va casi al final.
            modelBuilder.SeedPaymentAgreement();
            modelBuilder.SeedInstallmentSchedule();
        }


        public override int SaveChanges()
        {
            try
            {
                ChangeTracker.DetectChanges();
                //_auditService.CaptureAsync(ChangeTracker).GetAwaiter().GetResult();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SaveChanges: {ex.Message}");
                throw;
            }
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

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                ChangeTracker.DetectChanges();
                //await _auditService.CaptureAsync(ChangeTracker, cancellationToken);
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SaveChangesAsync: {ex.Message}");
                throw;
            }
        }
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
}