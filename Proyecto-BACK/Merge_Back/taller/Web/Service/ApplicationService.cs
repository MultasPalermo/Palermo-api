using Business.Custom;
using Business.ExternalServices.Recaptcha;
using Business.Interfaces.IBusinessImplements;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.IJWT;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Mensajeria.Implements;
using Business.Services;
using Business.Services.Entities;
using Business.Services.parameters;
using Business.Services.PDF;
using Business.Services.Security;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement.Entities;
using Data.Interfaces.IDataImplement.parameters;
using Data.Interfaces.IDataImplement.Security;
using Data.Interfaces.Security;
using Data.Repositoy;
using Data.Services;
using Data.Services.Entities;
using Data.Services.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Domain.Models.Implements.Recaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Utilities.Custom;
using Web.AutoMapper;
using Web.Configurations;
using Web.Infrastructure;
using Web.WebBackgroundService;

namespace Web.Service
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infra
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSingleton<EncriptePassword>();
            services.AddMemoryCache();

            // Persistencia genérica
            services.AddScoped(typeof(IData<>), typeof(DataGeneric<>));

            // Repositorios — PARAMETERS
            services.AddScoped<ImunicipalityRepository, municipalityRepository>();

            // Repositorios — SECURITY
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonRepository, personRepository>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();
            services.AddScoped<IFormModuleRepository, FormModuleRepository>();
            services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();

            // Repositorios — ENTITIES
            services.AddScoped<ITypeInfractionRepository, TypeInfractionRepository>();
            services.AddScoped<IInfractionRepository, InfractionRepository>();
            services.AddScoped<IDocumentInfractionRepository, DocumentInfractionRepository>();
            services.AddScoped<IPaymentAgreementRepository, PaymentAgreementRepository>();
            services.AddScoped<IInspectoraReportRepository, InspectoraReportRepository>();
            services.AddScoped<IUserInfractionRepository, UserInfractionRepository>();
            services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
            services.AddScoped<IFineCalculationDetailRepository, FineCalculationDetailsRepository>();
            services.AddScoped<IValueSmldvRepository, ValueSmldvRepository>();
            services.AddScoped<IInstallmentScheduleRepository, InstallmentScheduleRepository>();

            // Servicios — PARAMETERS
            services.AddScoped<IdepartmentServices, departmentServices>();
            services.AddScoped<ImunicipalityServices, municipalityServices>();
            services.AddScoped<IdocumentTypeServices, documentTypeServices>();
            services.AddScoped<IPaymentFrequencyServices, PaymentFrequencyServices>();
            services.AddScoped<ITypePaymentServices, TypePaymentServices>();

            // Servicios — SECURITY
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolUserService, RolUserService>();
            services.AddScoped<IFormModuleService, FormModuleService>();
            services.AddScoped<IRolFormPermissionService, RolFormPermissionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IToken, TokenBusiness>();
            services.AddScoped<IAuthSessionRepository, AuthSessionRepository>();
            services.AddScoped<IAuthSessionService, AuthSessionService>();
            services.AddSingleton<ISystemClock, SystemClock>();

            // Servicios — ENTITIES
            services.AddScoped<ITypeInfractionServices, TypeInfractionService>();
            services.AddScoped<IInfractionService, InfractionService>();
            services.AddScoped<IDocumentInfractionServices, DocumentInfractionServices>();
            services.AddScoped<IInspectoraReportService, InspectoraReportService>();
            services.AddScoped<IPaymentAgreementServices, PaymentAgreementServices>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserInfractionServices, UserInfractionServices>();
            services.AddScoped<IFineCalculationDetailService, FineCalculationDetailService>();
            services.AddScoped<IValueSmldvService, ValueSmldvService>();
            services.AddScoped<IInstallmentScheduleServices, InstallmentScheduleService>();

            // Servicios PDF
            services.AddScoped<IPdfGeneratorService, PdfService>();

            // Background Services
            services.AddHostedService<InfractionDiscountBackgroundService>();
            services.AddScoped<DiscountService>();
            services.AddHostedService<EmailBackgroundService>();
            services.AddSingleton<EmailBackgroundQueue>();
            services.AddScoped<IServiceEmail, ServiceEmails>();
            services.AddHostedService<PaymentAgreementBackgroundService>();

            // Recaptcha
            services.Configure<RecaptchaOptions>(configuration.GetSection("Recaptcha"));
            services.AddHttpClient<IRecaptchaVerifier, RecaptchaVerifier>();

            // Identity y Tokens
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserMeRepository, MeRepository>();
            services.AddScoped<IAuthCookieFactory, AuthCookieFactory>();
            services.AddScoped<IVerificationService, VerificationService>();
            services.AddSingleton<VerificationCache>();

            // Configuración adicional
            services.Configure<PaymentAgreementInterestOptions>(
                configuration.GetSection("PaymentAgreementInterestOptions"));

            return services;
        }
    }
}
