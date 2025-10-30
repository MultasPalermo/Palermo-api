using Entity.Domain.Models.Implements.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.Entities.UserNotification
{
    public class UserNotificationValidator : AbstractValidator<UserNotificationDto>
    {
        public UserNotificationValidator()

        {
            RuleFor(x => x.message)
                .NotEmpty().WithMessage("El mensaje es obligatorio.")
                .MaximumLength(500).WithMessage("El mensaje no debe superar los 500 caracteres.");

            RuleFor(x => x.shippingDate)
                .NotEmpty().WithMessage("La fecha de envío es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de envío no puede ser en el futuro.");
        }
    }

}
