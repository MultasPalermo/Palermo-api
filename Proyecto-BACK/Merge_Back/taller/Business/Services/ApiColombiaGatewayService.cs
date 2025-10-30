//using Business.Interfaces.IBusinessImplements;
//using Entity.DTOs.Select;
//using Newtonsoft.Json;

//namespace Business.Services
//{
//    public class ApiColombiaGatewayService : IApiColombiaGatewayService
//    {
//        private readonly HttpClient _httpClient;

//        public ApiColombiaGatewayService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<List<TouristicAttractionApiDto>> GetAttractionsAsync()
//        {
//            var response = await _httpClient.GetAsync("https://api-colombia.com/api/v1/TouristicAttraction");
//            response.EnsureSuccessStatusCode();
//            var json = await response.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<List<TouristicAttractionApiDto>>(json);
//        }
//    }


//}
