using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    // Entity/Domain/Models/Implements/ModelSecurity/AuthSession.cs
    public class AuthSession : BaseModel
    {
        public Guid SessionId { get; set; }
        public long? PersonId { get; set; }               // si mapeas persona
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public DateTime AbsoluteExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
    }

}
