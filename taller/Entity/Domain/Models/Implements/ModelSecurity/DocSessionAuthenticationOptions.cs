using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.ModelSecurity
{

    public class DocSessionAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string CookieName { get; set; } = "ph_session";
    }
}
