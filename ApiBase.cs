using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using VkNet;
using VkNet.Abstractions.Authorization;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;
namespace VK_R
{

    public class ApiBase
    {

        private static object _sync = new object();

        private VkApi vkApi;
        private static volatile ApiBase _instance;
        public event Action<Message, User> OnNewMessage;
        public event Action<long, bool> OnlineChanged;
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
            try
            {
                VkApi.Authorize(new ApiAuthParams
                {
                    ApplicationId = 2685278,
                    Login = login,
                    Password = password,
                    Settings = Settings.All | Settings.Offline | Settings.Friends | Settings.Messages
                });

            }
            catch (CaptchaNeededException ex)
            {
                var cpt = new System.Windows.Controls.Image { Source = new BitmapImage(ex.Img) };
                CaptchaWindow cptchaWindow = new CaptchaWindow(cpt);
                if (cptchaWindow.ShowDialog() == true)
                {
                    VkApi.Authorize(new ApiAuthParams
                    {
                        Login = login,
                        Password = password,
                        CaptchaSid = ex.Sid,
                        CaptchaKey = cptchaWindow.SolvedCaptcha.Text
                    });
                }



            }
        }
        public void StartMessagesHandling()
        {
            LongPollServerResponse longPoolServerResponse = VkApi.Messages.GetLongPollServer(needPts: true);
            ts = Convert.ToUInt64(longPoolServerResponse.Ts);
            pts = longPoolServerResponse.Pts;

            new Thread(LongPollEventLoop).Start();
        }
        private void LongPollEventLoop()
        {
            bool flag = true;
            while (true)
            {
                LongPollHistoryResponse lpr = null;
                flag = !flag;
                if (flag)
                {
                    lpr = VkApi.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams()
                    {
                        Ts = ts,
                        Pts = pts,
                        Fields = UsersFields.Photo50
                    });
                }
                else
                {
                    lpr = VkApi.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams()
                    {
                        Ts = ts,
                        Onlines = true,
                        Fields = UsersFields.Photo50
                    });
                }
                pts = lpr.NewPts;
                for (int i = 0; i < lpr.History.Count; i++)
                {
                    switch (lpr.History[i][0])
                    {
                        //Код 4 - новое сообщение
                        case 4:
                            var message = lpr.Messages.Where(x => x.PeerId == lpr.History[i][3]).FirstOrDefault();
                            var profile = lpr.Profiles.Where(x => x.Id == message.FromId).FirstOrDefault();
                            System.Windows.Application.Current.Dispatcher.Invoke(OnNewMessage, message, profile);
                            break;
                        case 8:
                            System.Windows.Application.Current.Dispatcher.Invoke(OnlineChanged, lpr.History[i][1] * -1, true);
                            break;
                        case 9:
                            System.Windows.Application.Current.Dispatcher.Invoke(OnlineChanged, lpr.History[i][1] * -1, false);
                            break;
                    }
                }
            }
        }

    }
}
