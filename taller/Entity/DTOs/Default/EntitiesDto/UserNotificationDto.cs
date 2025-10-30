using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class UserNotificationDto : IHasId
    {
        public int id {  get; set; }
        public string message {  get; set; }
        public DateTime shippingDate { get; set; } 
        public bool state { get; set; }
    }
}
