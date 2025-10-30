using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Repository;
using Data.Interfaces.IDataImplement.Entities;
using Data.Services.Entities;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Select.EntitiesSelectDto;
using Microsoft.Extensions.Logging;

namespace Business.Services.Entities
{
    public class TypeInfractionService
        : BusinessBasic<TypeInfractionDto, TypeInfractionSelectDto, TypeInfraction>, ITypeInfractionServices
    {
        private readonly ILogger<TypeInfractionService> _logger;
        private readonly ITypeInfractionRepository _repository;

        public TypeInfractionService(ITypeInfractionRepository repository, IMapper mapper, ILogger<TypeInfractionService> logger)
            : base(repository, mapper)
        {
            _repository = repository;
            _logger = logger;
        }

    }
}
