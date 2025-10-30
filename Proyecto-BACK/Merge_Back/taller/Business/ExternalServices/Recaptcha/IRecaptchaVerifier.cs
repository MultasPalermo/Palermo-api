using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ExternalServices.Recaptcha
{
    public interface IRecaptchaVerifier
    {
        Task<(bool ok, string? reason, double score)> VerifyAsync(
            string token, string expectedAction, string? remoteIp = null);
    }
}
