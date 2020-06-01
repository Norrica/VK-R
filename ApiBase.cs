using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static volatile ApiBase _instance;

        private VkApi vkApi;

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
#warning check if appId suitable
                ApplicationId = 2685278,
                Login = login,
                Password = password,
#warning find da way to pass only messages and related stuff
                Settings = Settings.All | Settings.Offline | Settings.Friends
            });
        }

    }
}
