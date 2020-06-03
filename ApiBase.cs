using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VkNet;
using VkNet.Abstractions.Authorization;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
namespace VK_R
{

    public class ApiBase
    {

        private static object _sync = new object();

        private VkApi vkApi;
        private static volatile ApiBase _instance;
        public event Action<Message> OnNewMessage;
        public event Action<Message, User> OnlineChanged;
        private ulong ts;
        private ulong? pts;

        public VkApi VkApi { get => vkApi; set => vkApi = value; }

        private ApiBase()
        {
            var serv = new ServiceCollection().AddAudioBypass();
            VkApi = new VkApi(serv);
        }
        public static ApiBase GetInstance()
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new ApiBase();
                    }
                }
            }
            if (_instance.VkApi == null)
            {
                throw new ArgumentNullException(nameof(_instance.VkApi));
            }
            return _instance;
        }
        public void Authorize(string login, string password)
        {
            if (VkApi.IsAuthorized)
                return;
            VkApi.Authorize(new ApiAuthParams
            {
                ApplicationId = 2685278,
                Login = login,
                Password = password,
                Settings = Settings.All | Settings.Offline | Settings.Friends
            });

        }
        public void StartMessagesHandling()
        {
            //Соединяемся с сервером Long Poll запросов и получаем необходимые ts и pts
            LongPollServerResponse longPoolServerResponse = VkApi.Messages.GetLongPollServer(needPts: true);
            ts = Convert.ToUInt64(longPoolServerResponse.Ts);
            pts = longPoolServerResponse.Pts;

            //В отдельном потоке запускаем метод, который будет постоянно опрашивать Long Poll сервер на наличие новых сообщений
            new Thread(LongPollEventLoop).Start();
        }
        private void LongPollEventLoop()
        {
            while (true)
            {
                LongPollHistoryResponse longPollResponse = VkApi.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams()
                {
                    Ts = ts,
                    Pts = pts,
                    Fields = UsersFields.Photo100 //Указывает поля, которые будут возвращаться для каждого профиля. В данном примере для каждого отправителя сообщения получаем фото 100х100
                });

                pts = longPollResponse.NewPts;
                for (int i = 0 , j=0; j < longPollResponse.History.Count && i<longPollResponse.Messages.Count; i++)
                {
                    switch (longPollResponse.History[i][0])
                    {
                        case 4://New Message
                            OnNewMessage?.Invoke(
                                longPollResponse.Messages[i]
                                //longPollResponse.Profiles
                                // .Where(u => u.Id == longPollResponse.Messages[i].UserId)
                                // .FirstOrDefault()
                            );                            
                            break;
                        case 8://Friend online
                            //longPollResponse.Profiles[i].On

                            break;
                        case 9://Friend Offline

                            break;
                        case 61://Friend started typing
                            break;
                        case 62://Chat started typing
                            break;
                    }
                }
            }
        }
        
        
    }
}
