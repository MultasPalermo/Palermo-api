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
        Task SendVerificationAsync(string email);

        Task SendEmailAsync(string email, VerificacionEmailBuilder builder);

        bool ValidateCode(string email, string code, string type);

        Task SendVerificationPasswordAsync(string email);

        Task SendEmailPasswordAsync(string email, PasswordResetEmailBuilder builder);
    }
}
