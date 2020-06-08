using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VK_R
{
    internal class MessagesViewModel : PropertyChangedBase
    {
        private ApiBase Api = ApiBase.GetInstance();
        private RelayCommand sendReply;
        private DialogModel currentDialog;
        private string quickReply;
        private ObservableCollection<MessagesModel> messages = new ObservableCollection<MessagesModel>();
        public string QuickReply { get => quickReply; set => SetField(ref quickReply, value); }
        public ObservableCollection<MessagesModel> Messages { get => messages; set => SetField(ref messages, value); }
        public MessagesViewModel(DialogModel dialog)
        {
            GetMessages(dialog);
            currentDialog = dialog;
            Api.OnNewMessage += UpdateMessageList;
        }

        private void UpdateMessageList(Message message, User user)
        {
            Messages.Add(
                   new MessagesModel(
                   message.Text,
                   $"{user.FirstName} {user.LastName}",
                   user.Photo50,
                   message.Date ?? message.Date.Value.ToLocalTime()));
            //throw new NotImplementedException();
        }

        private void GetMessages(DialogModel dialog)
        {
            var messages = Api.VkApi.Messages.GetHistory(new MessagesGetHistoryParams
            {
                Reversed = true,
                Extended = true,
                UserId = dialog.PeerId,
            }); ;
            var mesList = messages.Messages.ToList();
            var profList = messages.Users.ToList();
#warning redo with For loop. UPD - redo Done
            for (int i = 0; i < mesList.Count; i++)
            {
                var profile = profList.Where(x => x.Id == mesList[i].FromId).FirstOrDefault();
                Messages.Add(
                    new MessagesModel(
                    mesList[i].Text,
                    $"{profile.FirstName} {profile.LastName}",
                    profile.Photo50,                    
                    mesList[i].Date ?? mesList[i].Date.Value.ToLocalTime()));
            }          
            //throw new NotImplementedException();
        }

        
        public RelayCommand SendQuickReplyCommand
        {
            get =>
                (sendReply ?? (sendReply = new RelayCommand(SendQuickMessage, CanSendQuickMessage)));
            set => SetField(ref sendReply, value);

        }
        private void SendQuickMessage()
        {
            Api.VkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = currentDialog.PeerId,
                Message = QuickReply,
                RandomId = new Random().Next(),

            });
            QuickReply = String.Empty;           
        }
        private bool CanSendQuickMessage()
        {
            return !String.IsNullOrEmpty(QuickReply);
        }
    }
}