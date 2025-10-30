using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using Utilities.Exceptions;

namespace Business.Services.Entities
{
    public class InspectoraReportService
        : BusinessBasic<InspectoraReportDto, InspectoraReportSelectDto, InspectoraReport>, IInspectoraReportService
    {
        private readonly ILogger<InspectoraReportService> _logger;
        protected readonly IInspectoraReportRepository Data;

        public InspectoraReportService(
            IInspectoraReportRepository data,
            IMapper mapper,
            ILogger<InspectoraReportService> logger
        ) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }


    }
}
