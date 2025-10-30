using Entity.DTOs.Default.ModelSecurityDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.ModelSecurity.User
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres.");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .MaximumLength(150).WithMessage("El correo electrónico no debe superar los 150 caracteres.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

            RuleFor(x => x.password)
                .MaximumLength(100).WithMessage("La contraseña no debe superar los 100 caracteres.");

            //RuleFor(x => x.PersonId)
            //    .GreaterThan(0).When(x => x.PersonId.HasValue)
            //    .WithMessage("Debe proporcionar un ID válido de Persona.");
        }
    }

}
