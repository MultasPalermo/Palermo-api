using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;

namespace Business.Interfaces.IBusinessImplements.parameters
{
    public interface IdepartmentServices : IBusiness<departmentDto,departmentSelectDto>
    {
    }
}
