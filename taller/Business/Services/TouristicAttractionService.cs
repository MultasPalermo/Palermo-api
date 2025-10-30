//using AutoMapper;
//using Business.Interfaces.IBusinessImplements;
//using Data.Interfaces.DataBasic;
//using Entity.Domain.Models;
//using Entity.DTOs.Select;

//namespace Business.Services
//{
//    public class TouristicAttractionService : ITouristicAttractionService
//    {
//        private readonly IApiColombiaGatewayService _gateway;
//        private readonly IData<TouristicAttraction> _data;
//        private readonly IMapper _mapper;

//        public TouristicAttractionService(IApiColombiaGatewayService gateway, IData<TouristicAttraction> data, IMapper mapper)
//        {
//            _gateway = gateway;
//            _data = data;
//            _mapper = mapper;
//        }

//        public async Task SyncFromApiAsync()
//        {
//            var apiAttractions = await _gateway.GetAttractionsAsync();
//            var entities = apiAttractions.Select(dto => new TouristicAttraction
//            {
//                name = dto.name,
//                description = dto.description
//            }).ToList();

//            await _data.AddRangeAsync(entities);
//        }

//        public async Task<List<TouristicAttractionApiDto>> GetAllAsync()
//        {
//            var entities = await _data.GetAllAsync();
//            return _mapper.Map<List<TouristicAttractionApiDto>>(entities);
//        }

//        public async Task CreateAsync(TouristicAttractionApiDto dto)
//        {
//            var entity = _mapper.Map<TouristicAttraction>(dto);
//            await _data.CreateAsync(entity);
//        }
//    }


//}
