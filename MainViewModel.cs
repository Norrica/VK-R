using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VK_R
{
    class MainViewModel : PropertyChangedBase
    {
        private ApiBase Api = ApiBase.GetInstance();
        private AuthorizationControl authControl = new AuthorizationControl();
        private DialogControl dialogs;
        //private MessagesControl messages = new MessagesControl() { DataContext = new MessagesViewModel(); }
        private List<UserControl> controls = new List<UserControl>();
        private UserControl currentControl;
        public MainViewModel()
        {
            
            authControl.Authorized += SwapControls;
            if (Api.VkApi.IsAuthorized)
            {
                CurrentControl = new DialogControl() { DataContext = new DialogViewModel() }; ;
            }
            else
            {
                CurrentControl = AuthControl;
            }

        }

        private void SwapControls()
        {
            CurrentControl = new DialogControl() { DataContext = new DialogViewModel() }; ;
        }

        public AuthorizationControl AuthControl { get => authControl; set => SetField(ref authControl, value); }
        public DialogControl Dialogs { get => dialogs; set => SetField(ref dialogs, value); }
        public UserControl CurrentControl { get => currentControl; set => SetField(ref currentControl, value); }
        internal List<UserControl> Controls { get => controls; set => SetField(ref controls, value); }

    }
}