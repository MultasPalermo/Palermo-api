using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface IUserNotificationService : IBusiness<UserNotificationDto, UserNotificationSelectDto>
    {
    }
  
}
