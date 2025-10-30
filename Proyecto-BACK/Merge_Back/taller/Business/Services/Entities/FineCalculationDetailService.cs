    using AutoMapper;
    using Business.Interfaces.IBusinessImplements.Entities;
    using Business.Repository;
    using Data.Interfaces.IDataImplement.Entities;
    using Entity.Domain.Models.Implements.Entities;

    namespace Business.Services.Entities
    {
        public class FineCalculationDetailService :
            BusinessBasic<FineCalculationDetailDto, FineCalculationDetailSelectDto, FineCalculationDetail>,
            IFineCalculationDetailService
        {
            private readonly IFineCalculationDetailRepository _repository;
            private readonly IValueSmldvRepository _valueSmldvRepository;
            private readonly IInfractionRepository _typeInfractionRepository;
            private readonly IMapper _mapper;

            public FineCalculationDetailService(
                IFineCalculationDetailRepository repository,
                IMapper mapper,
                IValueSmldvRepository valueSmldvRepository,
                IInfractionRepository typeInfractionRepository
            ) : base(repository, mapper)
            {
                _repository = repository;
                _valueSmldvRepository = valueSmldvRepository;
                _typeInfractionRepository = typeInfractionRepository;
                _mapper = mapper;
            }

            public override async Task<FineCalculationDetailSelectDto?> GetByIdAsync(int id)
            {
                if (id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que cero.");

                var entity = await _repository.GetByIdAsync(id);
                return entity == null ? null : _mapper.Map<FineCalculationDetailSelectDto>(entity);
            }

            public async Task<FineCalculationDetail> CreateAsync(FineCalculationDetailDto dto)
            {
                Validate(dto);

                var valueSmldv = await _valueSmldvRepository.GetByIdAsync(dto.valueSmldvId)
                    ?? throw new InvalidOperationException("No existe el valor de SMLDV seleccionado.");

                var typeInfraction = await _typeInfractionRepository.GetByIdAsync(dto.typeInfractionId)
                    ?? throw new InvalidOperationException("No existe el tipo de infracción seleccionado.");

                // ✅ Se guarda el valor histórico del SMLDV usado al momento de crear
                dto.SmldvValueAtCreation = (decimal)valueSmldv.value_smldv;

                // ✅ amountToPay siempre basado en el valor histórico guardado
                dto.totalCalculation = typeInfraction.numer_smldv * dto.SmldvValueAtCreation;

                var entity = _mapper.Map<FineCalculationDetail>(dto);
                return await _repository.CreateAsync(entity);
            }

            public async Task<FineCalculationDetail> UpdateAsync(FineCalculationDetailDto dto)
            {
                if (dto.id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que cero.");

                Validate(dto);

                var existing = await _repository.GetByIdAsync(dto.id)
                    ?? throw new InvalidOperationException("No existe el cálculo a actualizar.");

                var typeInfraction = await _typeInfractionRepository.GetByIdAsync(dto.typeInfractionId)
                    ?? throw new InvalidOperationException("No existe el tipo de infracción seleccionado.");

                // ✅ Se mantiene el valor de SMLDV histórico para no afectar multas antiguas
                dto.SmldvValueAtCreation = existing.SmldvValueAtCreation;

                // ✅ Recalcular amountToPay pero con el valor histórico
                dto.totalCalculation = typeInfraction.numer_smldv * dto.SmldvValueAtCreation;

                var entity = _mapper.Map<FineCalculationDetail>(dto);
                await _repository.UpdateAsync(entity);

                return entity;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                if (id <= 0)
                    throw new ArgumentException("El Id debe ser mayor que cero.");

                return await _repository.DeleteAsync(id);
            }

            protected void Validate(FineCalculationDetailDto dto)
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "El cálculo no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(dto.formula))
                    throw new ArgumentException("La fórmula es obligatoria.");

                if (dto.valueSmldvId <= 0)
                    throw new ArgumentException("Debe asociarse un valor de SMLDV válido.");

                if (dto.typeInfractionId <= 0)
                    throw new ArgumentException("Debe asociarse un tipo de infracción válido.");
            }
        }
    }
