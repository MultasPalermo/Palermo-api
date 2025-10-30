using System;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class PaymentAgreementDataInit
    {
        public static void SeedPaymentAgreement(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            decimal smldv = 43500m;

            decimal baseAmount1 = Math.Round(smldv * 2 * 1.5m, 0);
            decimal baseAmount2 = Math.Round(smldv * 4 * 1.0m, 0);
            decimal baseAmount3 = Math.Round(smldv * 8 * 1.0m, 0);
            decimal baseAmount4 = Math.Round(smldv * 16 * 1.2m, 0);

            modelBuilder.Entity<PaymentAgreement>().HasData(
                new PaymentAgreement
                {
                    id = 1,
                    address = "Carrera 10 #45-20",
                    neighborhood = "Eduardo Santos",
                    AgreementDescription = "Acuerdo a 4 cuotas iguales.",
                    expeditionCedula = new DateTime(2020, 05, 10),
                    PhoneNumber = "3101234567",
                    Email = "user1@example.com",
                    AgreementStart = seedDate,
                    AgreementEnd = seedDate.AddMonths(4),
                    userInfractionId = 1,
                    paymentFrequencyId = 1,
                    typePaymentId = 1,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    BaseAmount = baseAmount1,
                    AccruedInterest = 0,
                    OutstandingAmount = baseAmount1,
                    Installments = 4,
                    MonthlyFee = baseAmount1 / 4
                },
                new PaymentAgreement
                {
                    id = 2,
                    address = "Carrera 1 #23-18",
                    neighborhood = "Panamá",
                    AgreementDescription = "Acuerdo a 2 cuotas iguales.",
                    expeditionCedula = new DateTime(2017, 01, 12),
                    PhoneNumber = "3009876543",
                    Email = "user2@example.com",
                    AgreementStart = seedDate,
                    AgreementEnd = seedDate.AddMonths(2),
                    userInfractionId = 2,
                    paymentFrequencyId = 2,
                    typePaymentId = 2,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    BaseAmount = baseAmount2,
                    AccruedInterest = 0,
                    OutstandingAmount = baseAmount2,
                    Installments = 2,
                    MonthlyFee = baseAmount2 / 2
                },
                new PaymentAgreement
                {
                    id = 3,
                    address = "Calle 20 #15-40",
                    neighborhood = "La Merced",
                    AgreementDescription = "Acuerdo a 8 cuotas iguales.",
                    expeditionCedula = new DateTime(2018, 03, 10),
                    PhoneNumber = "3015558888",
                    Email = "user3@example.com",
                    AgreementStart = seedDate,
                    AgreementEnd = seedDate.AddMonths(8),
                    userInfractionId = 3,
                    paymentFrequencyId = 2,
                    typePaymentId = 3,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    BaseAmount = baseAmount3,
                    AccruedInterest = 0,
                    OutstandingAmount = baseAmount3,
                    Installments = 8,
                    MonthlyFee = baseAmount3 / 8
                },
                new PaymentAgreement
                {
                    id = 4,
                    address = "Avenida 5 #45-12",
                    neighborhood = "San Martín",
                    AgreementDescription = "Acuerdo a 12 cuotas iguales.",
                    expeditionCedula = new DateTime(2019, 05, 22),
                    PhoneNumber = "3024449999",
                    Email = "user4@example.com",
                    AgreementStart = seedDate,
                    AgreementEnd = seedDate.AddMonths(12),
                    userInfractionId = 4,
                    paymentFrequencyId = 3,
                    typePaymentId = 1,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    BaseAmount = baseAmount4,
                    AccruedInterest = 0,
                    OutstandingAmount = baseAmount4,
                    Installments = 12,
                    MonthlyFee = baseAmount4 / 12
                }
            );
        }
    }
}
