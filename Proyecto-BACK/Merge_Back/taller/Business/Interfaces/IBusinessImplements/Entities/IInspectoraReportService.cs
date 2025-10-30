using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface IInspectoraReportService : IBusiness<InspectoraReportDto, InspectoraReportSelectDto>
    {
    }
}
