using System;
using System.Collections.ObjectModel;
using System.Linq;
using VkNet.Model.RequestParams;

namespace VK_R
{
    internal class MessagesViewModel : PropertyChangedBase
    {
        private ApiBase Api = ApiBase.GetInstance();
        private ObservableCollection<MessagesModel> messages = new ObservableCollection<MessagesModel>();
        public MessagesViewModel(DialogModel dialog)
        {
            GetMessages(dialog);
        }

        private void GetMessages(DialogModel dialog)
        {
            var messages = Api.VkApi.Messages.GetHistory(new MessagesGetHistoryParams
            {
                UserId = dialog.PeerId,

            });
            var mesList = messages.Messages.ToList();
            foreach (var message in mesList)
            {
                Messages.Add(
                    new MessagesModel(
                    message.Text,
                    dialog.PeerName,
                    dialog.ProfilePicture,
                    message.Date ?? message.Date.Value));
            }
            //throw new NotImplementedException();
        }

        public ObservableCollection<MessagesModel> Messages { get => messages; set => SetField(ref messages, value); }
    }
}