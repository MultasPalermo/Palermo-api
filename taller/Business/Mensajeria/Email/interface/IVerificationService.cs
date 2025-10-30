using Business.Mensajeria.Email.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.@interface

{
    public interface IVerificationService
    {
        Task SendVerificationAsync(string nombre, string email);
        bool ValidateCode(string email, string code);

        Task SendEmailAsync(string email, VerificacionEmailBuilder builder);

    }
}
