//using Business.Mensajeria.Interfaces;
//using Microsoft.Extensions.Configuration;

//namespace Business.Messaging.Implements
//{
//    public class ServiceTelegram : IServiceTelegram
//    {
//        private readonly string _botToken;
//        private readonly HttpClient _httpClient;

//        public ServiceTelegram(IConfiguration config)
//        {
//            _botToken = config["Telegram:Token"]!;
//            _httpClient = new HttpClient();
//        }

//        //public Task SendMessageAsync(string chatId, string message)
//        //{
//        //    throw new NotImplementedException();
//        //}
//        public async Task SendMessageAsync(string chatId, string message)
//        {
//            string url = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";

//            HttpResponseMessage response = await _httpClient.GetAsync(url);
//            if (!response.IsSuccessStatusCode)
//            {
//                string error = await response.Content.ReadAsStringAsync();
//                throw new Exception($"Error enviando mensaje Telegram: {error}");
//            }
//        }
//    }
//}