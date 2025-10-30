using AutoMapper;
using Entity.Domain.Models;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.Auth;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Default.Me;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Default.parameters;
using Entity.DTOs.Default.RegisterRequestDto;
using Entity.DTOs.Select;
using Entity.DTOs.Select.Entities;
using Entity.DTOs.Select.EntitiesSelectDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using FormDto = Entity.DTOs.Default.ModelSecurityDto.FormDto;
using RolUserDto = Entity.DTOs.Default.ModelSecurityDto.RolUserDto;

namespace Web.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Roles
            CreateMap<Rol, RolSelectDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();

            // Users
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserSelectDto>()
                .ForMember(p => p.TypeDocument,
                o => o.MapFrom(S => S.documentType != null ? S.documentType.name : null));

            CreateMap<RolUser, RolUserDto>().ReverseMap();
            CreateMap<RolUser, RolUserSelectDto>().ReverseMap();

            // Forms & Modules
            CreateMap<Form, FormSelectDto>().ReverseMap();
            CreateMap<Form, FormDto>().ReverseMap();
            CreateMap<Module, ModuleSelectDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Permission, PermissionSelectDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();

            // Persons
            CreateMap<Person, PersonSelectDto>().ReverseMap();
            CreateMap<PersonDto, Person>()
                .ForMember(d => d.firstName, o => o.MapFrom(s => s.firstName))
                .ForMember(d => d.lastName, o => o.MapFrom(s => s.lastName))
                .ReverseMap();

            // Register
            CreateMap<RegisterRequestDto, User>()
                .ForMember(d => d.email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.PasswordHash, o => o.MapFrom(s => s.password));
            CreateMap<RegisterDto, User>()
                .ForMember(d => d.email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.Person, o => o.Ignore());
            CreateMap<RegisterDto, Person>()
                .ForMember(d => d.firstName, o => o.MapFrom(s => s.firstName))
                .ForMember(d => d.lastName, o => o.MapFrom(s => s.lastName));

            // FormModule & RolFormPermission
            CreateMap<FormModule, FormModuleSelectDto>().ReverseMap();
            CreateMap<FormModule, FormModuleDto>().ReverseMap();
            CreateMap<RolFormPermission, RolFormPermissionDto>().ReverseMap();
            CreateMap<RolFormPermission, RolFormPermissionSelectDto>().ReverseMap();

            // Entities

            // ValueSmldv
            CreateMap<ValueSmldv, ValueSmldvDto>().ReverseMap();
            CreateMap<ValueSmldv, ValueSmldvSelectDto>().ReverseMap();

            CreateMap<TypePayment, TypePaymentDto>().ReverseMap();
            CreateMap<TypePayment, TypePaymentSelectDto>().ReverseMap();


            CreateMap<PaymentAgreement, PaymentAgreementDto>().ReverseMap();

            CreateMap<InstallmentSchedule, InstallmentScheduleDto>().ReverseMap();
            CreateMap<InstallmentSchedule, InstallmentScheduleSelectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.RemainingBalance, opt => opt.MapFrom(src => src.RemainingBalance))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid))
            .ReverseMap();


            CreateMap<PaymentAgreement, PaymentAgreementDto>()
                .ReverseMap()
                .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Installments))
                .ForMember(dest => dest.MonthlyFee, opt => opt.MapFrom(src => src.MonthlyFee));

            // Mapeo de InstallmentSchedule
            CreateMap<InstallmentSchedule, InstallmentScheduleDto>().ReverseMap();

            // Mapeo de PaymentAgreement a PaymentAgreementSelectDto
            CreateMap<PaymentAgreement, PaymentAgreementSelectDto>()
                .ForMember(d => d.PersonName,
                    o => o.MapFrom(s => s.userInfraction.User.Person != null
                        ? $"{s.userInfraction.User.Person.firstName} {s.userInfraction.User.Person.lastName}"
                        : string.Empty))
                .ForMember(d => d.DocumentNumber,
                    o => o.MapFrom(s => s.userInfraction.User.documentNumber ?? string.Empty))
                .ForMember(d => d.DocumentType,
                    o => o.MapFrom(s => s.userInfraction.User.documentType != null
                        ? s.userInfraction.User.documentType.name
                        : string.Empty))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.userInfraction.User.email ?? string.Empty))
                .ForMember(d => d.address, o => o.MapFrom(s => s.address))
                .ForMember(d => d.Neighborhood, o => o.MapFrom(s => s.neighborhood))
                .ForMember(d => d.Infringement, o => o.MapFrom(s => s.userInfraction.Infraction.description))
                .ForMember(d => d.TypeFine, o => o.MapFrom(s => s.userInfraction.Infraction.TypeInfraction.Name))
                .ForMember(d => d.ValorSMDLV, o => o.MapFrom(s => s.userInfraction.smldvValueAtCreation ?? 0))
                .ForMember(d => d.PaymentMethod,
                    o => o.MapFrom(s => s.TypePayment != null ? s.TypePayment.name : string.Empty))
                .ForMember(d => d.FrequencyPayment,
                    o => o.MapFrom(s => s.paymentFrequency.intervalPage))
                .ForMember(d => d.Installments, o => o.MapFrom(s => s.Installments))
                .ForMember(d => d.MonthlyFee, o => o.MapFrom(s => s.MonthlyFee))
                .ForMember(d => d.InstallmentSchedule, o => o.MapFrom(s => s.InstallmentSchedule));


            // Document & Report
            CreateMap<DocumentInfraction, DocumentInfractionDto>().ReverseMap();
            CreateMap<DocumentInfraction, DocumentInfractionSelectDto>()
                .ForMember(d => d.inspectoraReportName,
                    o => o.MapFrom(s => s.inspectoraReport != null ? s.inspectoraReport.message : null))
                .ForMember(d => d.PaymentAgreementName,
                    o => o.MapFrom(s => s.paymentAgreement != null ? s.paymentAgreement.AgreementDescription : null));

            CreateMap<InspectoraReport, InspectoraReportDto>().ReverseMap();
            CreateMap<InspectoraReport, InspectoraReportSelectDto>().ReverseMap();
            CreateMap<InspectoraReport, InspectoraPdfDto>().ReverseMap();

            // FineCalculationDetail
            CreateMap<FineCalculationDetail, FineCalculationDetailDto>().ReverseMap();

            CreateMap<FineCalculationDetail, FineCalculationDetailSelectDto>()
                .ForMember(dest => dest.valueSmldvValue, opt => opt.MapFrom(src =>
                    (double?)src.SmldvValueAtCreation)) 
                .ForMember(dest => dest.currentYear, opt => opt.MapFrom(src =>
                    src.valueSmldv != null ? src.valueSmldv.Current_Year : 0))
                .ForMember(dest => dest.minimunWage, opt => opt.MapFrom(src =>
                    src.valueSmldv != null ? src.valueSmldv.minimunWage : 0))
                .ForMember(dest => dest.TypeInfractionName, opt => opt.MapFrom(src =>
                    src.Infraction != null && src.Infraction.TypeInfraction != null
                        ? src.Infraction.TypeInfraction.Name
                        : string.Empty))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src =>
                    src.Infraction != null ? src.Infraction.description : string.Empty))
                .ForMember(dest => dest.totalCalculation, opt => opt.MapFrom(src => src.totalCalculation))
                .ReverseMap();

            // 🔹 NUEVOS MAPPINGS TypeInfraction & Infraction
            CreateMap<TypeInfraction, TypeInfractionDto>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ReverseMap();

            CreateMap<TypeInfraction, TypeInfractionSelectDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ReverseMap();
            CreateMap<Infraction, InfractionDto>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.type_Infraction, o => o.MapFrom(s => s.TypeInfraction != null ? s.TypeInfraction.Name : string.Empty))
                .ForMember(d => d.numer_smldv, o => o.MapFrom(s => s.numer_smldv))
                .ForMember(d => d.description, o => o.MapFrom(s => s.description))
                .ReverseMap();

            CreateMap<Infraction, InfractionSelectDto>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.TypeInfractionName, o => o.MapFrom(s => s.TypeInfraction != null ? s.TypeInfraction.Name : string.Empty))
                .ForMember(d => d.numer_smldv, o => o.MapFrom(s => s.numer_smldv))
                .ForMember(d => d.description, o => o.MapFrom(s => s.description))
                .ReverseMap();

            // UserInfraction -> UserInfractionDto
            CreateMap<UserInfraction, UserInfractionDto>()
                .ForMember(d => d.userId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.userEmail, o => o.MapFrom(s => s.User != null && s.User.email != null ? s.User.email : string.Empty))
                .ForMember(d => d.documentNumber, o => o.MapFrom(s => s.User != null && s.User.documentNumber != null ? s.User.documentNumber : string.Empty))
                .ForMember(d => d.typeInfractionId, o => o.MapFrom(s => s.InfractionId))
                .ForMember(d => d.stateInfraction, o => o.MapFrom(s => s.stateInfraction))
                .ForMember(d => d.dateInfraction, o => o.MapFrom(s => s.dateInfraction))
                .ForMember(d => d.observations, o => o.MapFrom(s => s.Infraction.description ?? string.Empty))
                .ForMember(d => d.amountToPay, o => o.MapFrom(s => s.amountToPay))
                .ForMember(d => d.smldvValueAtCreation, o => o.MapFrom(s => s.smldvValueAtCreation))  // ✅ Nuevo campo
                .ForMember(d => d.UserNotificationId, o => o.MapFrom(s => s.UserNotificationId))
                .ReverseMap();

            // UserInfraction -> UserInfractionSelectDto
            CreateMap<UserInfraction, UserInfractionSelectDto>()
                .ForMember(d => d.userId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.userEmail, o => o.MapFrom(s => s.User != null ? s.User.email : string.Empty))
                .ForMember(d => d.firstName, o => o.MapFrom(s => s.User != null && s.User.Person != null ? s.User.Person.firstName : string.Empty))
                .ForMember(d => d.lastName, o => o.MapFrom(s => s.User != null && s.User.Person != null ? s.User.Person.lastName : string.Empty))
                .ForMember(d => d.documentNumber, o => o.MapFrom(s => s.User != null ? s.User.documentNumber : string.Empty))
                .ForMember(d => d.typeInfractionId, o => o.MapFrom(s => s.InfractionId))
                .ForMember(d => d.typeInfractionName, o => o.MapFrom(s => s.Infraction != null && s.Infraction.TypeInfraction != null ? s.Infraction.TypeInfraction.Name : string.Empty))
                .ForMember(d => d.stateInfraction, o => o.MapFrom(s => s.stateInfraction))
                .ForMember(d => d.dateInfraction, o => o.MapFrom(s => s.dateInfraction))
                .ForMember(d => d.observations, o => o.MapFrom(s => s.Infraction.description ?? string.Empty))
                .ForMember(d => d.amountToPay, o => o.MapFrom(s => s.amountToPay))
                .ForMember(d => d.smldvValueAtCreation, o => o.MapFrom(s => s.smldvValueAtCreation))  // ✅ Nuevo campo
                .ForMember(d => d.UserNotificationId, o => o.MapFrom(s => s.UserNotificationId));


            // Parameters
            CreateMap<department, departmentDto>().ReverseMap();
            CreateMap<department, departmentSelectDto>().ReverseMap();
            CreateMap<municipality, municipalityDto>().ReverseMap();
            CreateMap<municipality, municipalitySelectDto>().ReverseMap();
            CreateMap<PaymentFrequency, PaymentFrequencyDto>().ReverseMap();
            CreateMap<PaymentFrequency, PaymentFrequencySelectDto>().ReverseMap();
            CreateMap<documentType, documentTypeDto>().ReverseMap();
            CreateMap<documentType, documentTypeSelectDto>().ReverseMap();

            // Me
            CreateMap<User, UserMeDto>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.firstName, o => o.MapFrom(s => s.Person.firstName))
                .ForMember(d => d.lastName, o => o.MapFrom(s => s.Person.lastName))
                .ForMember(d => d.fullName, o => o.MapFrom(s => s.Person.firstName + " " + s.Person.lastName))
                .ForMember(d => d.roles, o => o.MapFrom(s => s.rolUsers.Select(r => r.Rol.name)))
                .ForMember(d => d.permissions, o => o.Ignore())
                .ForMember(d => d.Menu, o => o.Ignore());

            CreateMap<Module, MenuModuleDto>()
                .ForMember(dest => dest.forms,
                           opt => opt.MapFrom(src => src.FormModules.Select(fm => fm.form)));
            CreateMap<Form, FormMeDto>();
        }
    }
}
