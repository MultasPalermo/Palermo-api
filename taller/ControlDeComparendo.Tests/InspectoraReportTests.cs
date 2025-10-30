using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class InspectoraReportTests
    {
        [Fact]
        public void InspectoraReport_Should_Have_Default_Empty_List()
        {
            var report = new InspectoraReport
            {
                id = 1,
                report_date = DateTime.UtcNow,
                total_fines = 50000,
                message = "Reporte semanal"
            };

            Assert.Equal(1, report.id);
            Assert.Equal(50000, report.total_fines);
            Assert.NotNull(report.documentInfraction);
            Assert.Empty(report.documentInfraction);
        }
    }
}
