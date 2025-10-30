using Entity.Domain.Enums;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.Entities;

namespace Entity.Domain.Models.Implements.Entities
{
    public class UserInfractionDto : IHasId, IUserInfraction
    {
        public int id { get; set; }
        public DateTime dateInfraction { get; set; }
        public EstadoMulta stateInfraction {  get; set; }
        public string observations { get; set; }
        public int userId { get; set; }
        public int typeInfractionId {get; set; }
        public int UserNotificationId { get; set; }

        public string documentNumber { get; set; }

        public decimal amountToPay { get; set; }
        public decimal smldvValueAtCreation { get; set; }


        public string? userEmail { get; set; }

    }
}
