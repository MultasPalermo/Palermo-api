using Entity.Domain.Enums;
using Entity.Domain.Interfaces;
using System;

namespace Entity.DTOs.Interface.Entities
{
    public interface IUserInfraction : IHasId
    {
        DateTime dateInfraction { get; set; }
        EstadoMulta stateInfraction { get; set; }
        string observations { get; set; }
        int userId { get; set; }
        int typeInfractionId { get; set; }
        int UserNotificationId { get; set; }
    }
}
