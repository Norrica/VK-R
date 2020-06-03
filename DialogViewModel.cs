﻿using System;
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
            GetDialogs();
            Api.StartMessagesHandling();

//#error handle messages here
        }
        public void GetDialogs()
        {
            //throw new NotImplementedException();
            var dials = Api.VkApi.Messages.GetConversations(new GetConversationsParams
            {
                Extended = true,
                Fields = new[] {"photo_100","last_seen","online" }
            });
            for (int i = 0,j=0; i < dials.Items.Count && j < dials.Profiles.Count; i++)
            {
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.Chat)
                {
                    Dialogs.Add(new DialogModel(
                    dials.Items[i].Conversation.ChatSettings.Title,
                    dials.Items[i].Conversation.Peer.Id,
                    dials.Items[i].LastMessage.Text,
                    dials.Items[i].Conversation.ChatSettings.Photo?.Photo100 ?? new Uri("https://vk.com/images/camera_100.png"),
                    dials.Items[i].Conversation.ChatSettings.MembersCount));
                }
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.User)
                {
                    var profile = dials.Profiles.FirstOrDefault(x => x.Id == dials.Items[i].Conversation.Peer.Id);
                    Dialogs.Add(new DialogModel(
                    $"{ profile.LastName } { profile.FirstName}",
                    profile.Id,
                    //dials.Items[i].Conversation.Peer.Id,
                    dials.Items[i].LastMessage.Text,
                    profile.Photo100 ?? new Uri("https://vk.com/images/camera_100.png"),
                    new Online(
                        profile.Online,
                        profile.LastSeen?.Time ?? new DateTime(1)
                        )
                    ));
                }
                if (dials.Items[i].Conversation.Peer.Type == ConversationPeerType.Group)
                {
                    var group = dials.Groups.FirstOrDefault(x => x.Id == dials.Items[i].Conversation.Peer.LocalId);
                    Dialogs.Add(new DialogModel(
                    $"{group.Name}",
                    dials.Items[i].Conversation.Peer.Id,
                    dials.Items[i].LastMessage.Text,
#warning groups can't be online, fix dat.UPD:kinda fixed, but my eyes bleed
                    group.Photo100 ?? new Uri("https://vk.com/images/camera_100.png")));
                    

                }
            }
        }
    }
}