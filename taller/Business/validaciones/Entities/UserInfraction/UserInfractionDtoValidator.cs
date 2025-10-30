using System;
using Entity.DTOs.Interface.Entities;
using FluentValidation;

namespace Business.validaciones.Entities.UserInfraction
{
    public class UserInfractionDtoValidator<T> : AbstractValidator<T> where T : IUserInfraction
    {
        public UserInfractionDtoValidator()
        {

            // 🔹 Observaciones
            RuleFor(x => x.observations)
                .MaximumLength(500).WithMessage("Las observaciones no pueden superar los 500 caracteres.");

            // 🔹 userId válido
            RuleFor(x => x.userId)
                .GreaterThan(0).WithMessage("El usuario asociado a la infracción es obligatorio.");

            // 🔹 typeInfractionId válido
            RuleFor(x => x.typeInfractionId)
                .GreaterThan(0).WithMessage("El tipo de infracción es obligatorio.");

            // 🔹 UserNotificationId válido
            RuleFor(x => x.UserNotificationId)
                .GreaterThan(0).WithMessage("La notificación de usuario es obligatoria.");
        }
    }
}
