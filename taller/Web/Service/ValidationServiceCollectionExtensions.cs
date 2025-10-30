using Business.validaciones.Auth;
using Business.validaciones.Entities.DocumentInfraction;
using Business.validaciones.Entities.FineCalculationDetail;
using Business.validaciones.Entities.InspectoraReport;
using Business.validaciones.Entities.PaymentAgreement;
using Business.validaciones.Entities.TypeInfraction;
using Business.validaciones.Entities.TypePayment;
using Business.validaciones.Entities.UserInfraction;
using Business.validaciones.Entities.ValueSmldv;
using Business.validaciones.ModelSecurity.Form;
using Business.validaciones.ModelSecurity.FormModule;
using Business.validaciones.ModelSecurity.Module;
using Business.validaciones.ModelSecurity.Permission;
using Business.validaciones.ModelSecurity.Person;
using Business.validaciones.Parameter.department;
using Business.validaciones.Parameter.municipality;
using Entity.Domain.Models.Implements.ModelSecurity;
using FluentValidation;

namespace Web.Service
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            // Entities
            services.AddValidatorsFromAssemblyContaining<InspectoraReportCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<DocumentInfractionCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<PaymentAgreementCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<TypePaymentCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<FineCalculationDetailValidator>();
            services.AddValidatorsFromAssemblyContaining<TypeInfractionValidator>();
            services.AddValidatorsFromAssemblyContaining<ValueSmldvValidator>();
            services.AddValidatorsFromAssemblyContaining<UserInfracionCreateValidator>();

            // Parameters
            services.AddValidatorsFromAssemblyContaining<departmentCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<municipalityCreateValidator>();

            // ModelSecurity
            services.AddValidatorsFromAssemblyContaining<FormCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<ModuloCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<PermissionCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<PersonCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<FormModuleCreateValidator>();

            //Auth
            services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<DocumentDtoValidator>();

            return services;
        }
    }
}

