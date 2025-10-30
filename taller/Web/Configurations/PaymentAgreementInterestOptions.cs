namespace Web.Configurations
{
    public class PaymentAgreementInterestOptions
    {
        public decimal MonthlyRate { get; set; } = 0.02m; // 2% mensual
        public int GracePeriodDays { get; set; } = 30;    // días antes de coactivo
        public int DailyDivisor { get; set; } = 30;       // prorrateo mensual a días (30)
    }

}
