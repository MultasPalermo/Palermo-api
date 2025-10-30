using Entity.DTOs.Default.RegisterRequestDto;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Business.validaciones.Auth
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            // Nombre completo
            RuleFor(x => x.NombreCompleto)
                .NotEmpty().WithMessage("El nombre completo es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre completo no debe superar los 150 caracteres.")
                // al menos nombre y apellido
                .Must(n =>
                {
                    var parts = n?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    return parts.Length >= 2;
                })
                .WithMessage("Debe ingresar al menos Nombre y apellido")
                .Must(n =>
                {
                    var parts = n?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    return parts.Length >= 2 && parts.Last().Length >= 2;
                })
                .WithMessage("Debe ingresar un apellido valido al final");

            // Email
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(150).WithMessage("El correo electrónico no debe superar los 150 caracteres.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase)
                    .WithMessage("El correo debe ser un Gmail válido con formato usuario@gmail.com.");


            // Password
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .MaximumLength(100).WithMessage("La contraseña no debe superar los 100 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe incluir al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe incluir al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe incluir al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe incluir al menos un carácter especial.");
        }
    }
}
