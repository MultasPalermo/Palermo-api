using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Repository;
using Business.Services.Entities;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement.parameters;
using Data.Services.Security;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services.parameters
{
    public class PaymentFrequencyServices : BusinessBasic<PaymentFrequencyDto,PaymentFrequencySelectDto,PaymentFrequency>,IPaymentFrequencyServices
    {
        private readonly ILogger<PaymentFrequencyServices> _logger;
        private readonly IPaymentFrequencyRepository _repository;

        public PaymentFrequencyServices(
            IPaymentFrequencyRepository repository,
            IMapper mapper,
            ILogger<PaymentFrequencyServices> logger)
            : base(repository, mapper)
        {
            _repository = repository;
            _logger = logger;
        }


        // Obtener frecuencia por intervalo
        public async Task<PaymentFrequency?> GetByIntervalAsync(string interval)
        {
            return await _repository.GetByIntervalAsync(interval);
        }

        // CALCULAR SIGUIENTE FECHA 
        public async Task<DateTime> CalculateNextDateAsync(DateTime currentDate, string frequencyInterval)
        {
            var frequency = await GetByIntervalAsync(frequencyInterval);
            if (frequency == null)
                throw new BusinessException($"Frecuencia '{frequencyInterval}' no encontrada en la base de datos.");

            // Validar configuración
            if (string.IsNullOrWhiteSpace(frequency.IntervalType))
                throw new BusinessException($"La frecuencia '{frequencyInterval}' no tiene configurado el campo IntervalType.");

            if (frequency.IntervalValue <= 0)
                throw new BusinessException($"La frecuencia '{frequencyInterval}' tiene un IntervalValue inválido: {frequency.IntervalValue}");

            // ✅ CALCULAR SIGUIENTE FECHA DINÁMICAMENTE SEGÚN IntervalType
            DateTime nextDate = frequency.IntervalType.ToUpper() switch
            {
                "DAYS" => currentDate.AddDays(frequency.IntervalValue),
                "MONTHS" => currentDate.AddMonths(frequency.IntervalValue),
                "YEARS" => currentDate.AddYears(frequency.IntervalValue),
                _ => throw new BusinessException(
                    $"Tipo de intervalo '{frequency.IntervalType}' no soportado. " +
                    $"Valores válidos: Days, Months, Years, Hours, Minutes, Seconds")
            };


            _logger.LogDebug(
                "Siguiente fecha calculada: {NextDate} (Frecuencia: {Frequency}, Tipo: {Type}, Valor: {Value})",
                nextDate, frequencyInterval, frequency.IntervalType, frequency.IntervalValue);

            return nextDate;
        }

        //CALCULAR FECHA FINAL CON CUOTAS
        public async Task<DateTime> CalculateEndDateAsync(DateTime startDate, string frequencyInterval, int installments)
        {
            if (installments <= 0)
                throw new BusinessException("La cantidad de cuotas debe ser mayor a cero.");

            _logger.LogInformation(
                "Calculando fecha final: startDate={StartDate}, frequency={Frequency}, installments={Installments}",
                startDate, frequencyInterval, installments);

            var nextPaymentDate = startDate;

            for (int i = 0; i < installments; i++)
            {
                nextPaymentDate = await CalculateNextDateAsync(nextPaymentDate, frequencyInterval);
            }

            _logger.LogInformation(
                "Fecha final calculada: {EndDate} para {Installments} cuotas",
                nextPaymentDate, installments);

            return nextPaymentDate;
        }
    }
}
