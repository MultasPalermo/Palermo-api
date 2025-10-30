using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class UserNotification : BaseModel
    {
        public string message {  get; set; }
        public DateTime shippingDate { get; set; }

        //relaciones
        public List<UserInfraction> userInfraction { get; set; } = new List<UserInfraction>();
    }
}
