//using Business.Mensajeria.Interfaces;
//using Microsoft.Extensions.Configuration;

//namespace Business.Messaging.Implements
//{
//    public class NotifyManager : INotifyManager
//    {
//        private readonly IServiceTelegram _serviceTelegram;
//        private readonly IConfiguration _config;
//        public NotifyManager(IServiceTelegram serviceTelegram, IConfiguration config)
//        {
//            _serviceTelegram = serviceTelegram;
//            _config = config;

//        }

//        public string message = "Inicio de sesion detectado";
//        public async Task NotifyAsync()
//        {
//            await _serviceTelegram.SendMessageAsync(_config["Telegram:ChatId"]!, message);
//        }
//    }
//}
