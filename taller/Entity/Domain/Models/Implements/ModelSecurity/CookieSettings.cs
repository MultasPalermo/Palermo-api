using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class CookieSettings
    {
        public string accessTokenName { get; set; } = "access_token";
        public string refreshTokenName { get; set; } = "refresh_token";
        public string csrfCookieName { get; set; } = "XSRF-TOKEN";
        public string path { get; set; } = "/";
        public string? domain { get; set; } = null;
        public bool secure { get; set; } = true;
        public SameSiteMode sameSiteMode { get; set; } = SameSiteMode.None;
    }
}
