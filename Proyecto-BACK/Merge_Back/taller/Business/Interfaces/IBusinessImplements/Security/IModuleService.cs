using Business.Interfaces.BusinessBasic;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;

namespace Business.Interfaces.IBusinessImplements.Security
{
    public interface IModuleService : IBusiness<ModuleDto, ModuleSelectDto>
    {
    }
}
