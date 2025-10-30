using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.Entities;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface ITypePaymentServices : IBusiness<TypePaymentDto,TypePaymentSelectDto>
    {
    }
}
