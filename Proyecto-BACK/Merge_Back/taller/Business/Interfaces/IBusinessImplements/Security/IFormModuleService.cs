using Business.Interfaces.BusinessBasic;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;

namespace Business.Interfaces.IBusinessImplements
{
    public interface IFormModuleService : IBusiness<FormModuleDto, FormModuleSelectDto>
    {
    }
}
