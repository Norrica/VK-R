using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace VK_R
{
    internal class DialogViewModel : PropertyChangedBase
    {
        ApiBase Api = ApiBase.GetInstance();
        private ObservableCollection<DialogModel> dialogs = new ObservableCollection<DialogModel>();

        public ObservableCollection<DialogModel> Dialogs { get => dialogs; set => SetField(ref dialogs, value); }

        public DialogViewModel()
        {
            GetDialogs();//Call if authorized
        }
        public void GetDialogs()//fetch from server or locally
        {
            //throw new NotImplementedException();
            var dials = Api.VkApi.Messages.GetConversations(new GetConversationsParams
            {
                Extended = true,
            });
            for (int i = 0,j=0; i < dials.Items.Count && j < dials.Profiles.Count; i++)
            {
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.Chat)
                {
                    Dialogs.Add(new DialogModel(
                    dials.Items[i].Conversation.ChatSettings.Title,
                    dials.Items[i].LastMessage.Text,
                    dials.Items[i].LastMessage.Date,
                    dials.Items[i].Conversation.ChatSettings.Photo?.Photo100 ?? new Uri("https://vk.com/images/camera_100.png")));
                }
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.User)
                {
                    var profile = dials.Profiles.FirstOrDefault(x => x.Id == dials.Items[i].Conversation.Peer.Id);
                    Dialogs.Add(new DialogModel(
                    $"{ profile.LastName } { profile.FirstName}",
                    dials.Items[i].LastMessage.Text,
                    dials.Items[i].LastMessage.Date,
                    profile.Photo100?? new Uri("https://vk.com/images/camera_100.png")));
                }
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.Group)
                {
                    var group = dials.Groups.FirstOrDefault(x => x.Id == dials.Items[i].Conversation.Peer.LocalId);
                    Dialogs.Add(new DialogModel(
                    $"{group.Name}",
                    dials.Items[i].LastMessage.Text,
                    dials.Items[i].LastMessage.Date,
                    group.Photo100 ?? new Uri("https://vk.com/images/camera_100.png")));


                }
            }
        }
    }
}