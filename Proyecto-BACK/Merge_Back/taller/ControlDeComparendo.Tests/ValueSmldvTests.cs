using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class ValueSmldvTests
    {
        [Fact]
        public void Can_Create_ValueSmldv_With_Properties()
        {
            var smldv = new ValueSmldv
            {
                id = 1,
                value_smldv = 150000.50m,  
                Current_Year = 2025,
                minimunWage = 1300000m    
            };

            Assert.Equal(1, smldv.id);
            Assert.Equal(150000.50m, smldv.value_smldv); 
            Assert.Equal(2025, smldv.Current_Year);
            Assert.Equal(1300000m, smldv.minimunWage);   
            Assert.Null(smldv.fineCalculationDetail);  
        }
    }
}
