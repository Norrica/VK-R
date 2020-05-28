using System;
using System.Collections.ObjectModel;
using System.Windows;
using VkNet.Model.RequestParams;

namespace VK_R
{
    internal class DialogViewModel:PropertyChangedBase
    {
        ApiBase Api = ApiBase.GetInstance();
        private ObservableCollection<DialogModel> dialogs = new ObservableCollection<DialogModel> { new DialogModel(), new DialogModel() };

        public ObservableCollection<DialogModel> Dialogs { get => dialogs; set => SetField(ref dialogs, value); }

        public void GetDialogs()//fetch from server or locally
        {
            throw new NotImplementedException();
            var dials = Api.VkApi.Messages.GetConversations(new GetConversationsParams { 
            Extended=true,
            Count=5
            });

            foreach (var item in dials.Items)
            {
                MessageBox.Show(item.LastMessage.Date.ToString());
            }
        }
        public DialogViewModel()
        {
            //GetDialogs();
        }
    }
}