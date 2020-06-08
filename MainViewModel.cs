using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VkNet.Model.RequestParams;

namespace VK_R
{
    class MainViewModel : PropertyChangedBase
    {
        private ApiBase Api = ApiBase.GetInstance();
        private AuthorizationControl authControl = new AuthorizationControl();
        private DialogControl dialogs;
        private DialogViewModel dvm;
        private MessagesViewModel mvm;
        private UserControl messages;
        private UserControl currentControl;
        public MainViewModel()
        {
            var cont = SynchronizationContext.Current;
            authControl.Authorized += SwapControls;
            if (Api.VkApi.IsAuthorized)
            {
                dvm = new DialogViewModel();
                dvm.SelectionChanged += CreateMessageView;
                CurrentControl = new DialogControl() { DataContext = dvm }; ;
            }
            else
            {
                CurrentControl = AuthControl;
            }

        }

        private void CreateMessageView(DialogModel obj)
        {
            mvm = new MessagesViewModel(obj);
            Messages = new MessagesControl { DataContext = mvm };

           // throw new NotImplementedException();
        }

        private void SwapControls()
        {
            dvm = new DialogViewModel();
            dvm.SelectionChanged += CreateMessageView;
            CurrentControl = new DialogControl() { DataContext = dvm };
        }

        public AuthorizationControl AuthControl { get => authControl; set => SetField(ref authControl, value); }
        public DialogControl Dialogs { get => dialogs; set => SetField(ref dialogs, value); }
        public UserControl CurrentControl { get => currentControl; set => SetField(ref currentControl, value); }
        public UserControl Messages { get => messages; set => SetField(ref messages, value); }
    }
}