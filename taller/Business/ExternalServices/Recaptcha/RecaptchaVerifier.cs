using Entity.Domain.Models.Implements.Recaptcha;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Business.ExternalServices.Recaptcha
{
    public class RecaptchaVerifier : IRecaptchaVerifier
    {
        private readonly HttpClient _http;
        private readonly RecaptchaOptions _opt;

        public RecaptchaVerifier(HttpClient http, IOptions<RecaptchaOptions> opt)
        {
            _http = http;
            _opt = opt.Value;
        }

        public async Task<(bool ok, string? reason, double score)> VerifyAsync(
            string token, string expectedAction, string? remoteIp = null)
        {
            var form = new Dictionary<string, string>
            {
                ["secret"] = _opt.SecretKey,
                ["response"] = token
            };
            if (!string.IsNullOrWhiteSpace(remoteIp))
                form["remoteip"] = remoteIp;

            using var req = new HttpRequestMessage(HttpMethod.Post,
                "https://www.google.com/recaptcha/api/siteverify")
            {
                Content = new FormUrlEncodedContent(form)
            };

            var resp = await _http.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var data = await resp.Content.ReadFromJsonAsync<RecaptchaVerifyResponse>();

            Console.WriteLine($"[reCAPTCHA] success:{data?.Success}, score:{data?.Score}, action:{data?.Action}, host:{data?.Hostname}");

            if (data is null || !data.Success)
                return (false, $"recaptcha_failed:{string.Join(",", data?.ErrorCodes ?? Array.Empty<string>())}", 0);

            // Verifica que la acción coincida
            if (!string.Equals(data.Action, expectedAction, StringComparison.Ordinal))
                return (false, "action_mismatch", data.Score);

            // Aplica tu umbral
            if (data.Score < _opt.MinScore)
                return (false, "low_score", data.Score);

            return (true, null, data.Score);
        }
    }
}
