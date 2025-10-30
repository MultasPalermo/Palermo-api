using Business.Interfaces.BusinessBasic;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Select.EntitiesSelectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface ITypeInfractionServices : IBusiness<TypeInfractionDto, TypeInfractionSelectDto>
    {
    }
}
