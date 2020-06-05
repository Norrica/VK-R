﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using Image = System.Windows.Controls.Image;

namespace VK_R
{
    internal class DialogModel : PropertyChangedBase
    {
        ApiBase API = ApiBase.GetInstance();
        private string peerName;
        private long peerId;
        private string lastMessage;
        private Image profilePicture;
        private AttachmentContent attachment;
        private Image imgAttachment;
        private Online online;
        private string quickReply;
        private RelayCommand sendReply;

        public string PeerName { get => peerName; set => peerName = value; }
        public long PeerId { get => peerId; set => peerId = value; }
        public string LastMessage { get => lastMessage; set => SetField(ref lastMessage, value); }
        public Image ProfilePicture { get => profilePicture; set => profilePicture = value; }
        public AttachmentContent Attachment { get => attachment; set => SetField(ref attachment, value); }
        public Image ImgAttachment { get => imgAttachment; set => SetField(ref imgAttachment ,value); }
        public Online Online { get => online; set => SetField(ref online, value); }
        public string QuickReply { get => quickReply; set => SetField(ref quickReply, value); }
        public DialogModel()
        {
            API.OnNewMessage += HandleNewMessage;
            API.OnlineChanged += HandleOnlineChanged;
        }
        public DialogModel(Image attachment) : this()
        {
            ImgAttachment = attachment;
        }
        //for Chats
        public DialogModel(string peerName, long peerId, string lastMessage, Uri avatarUrl) : this()
        {
            PeerName = peerName;
            PeerId = peerId;
            LastMessage = lastMessage;
            ProfilePicture = new Image { Source = new BitmapImage(avatarUrl) };
        }
        public DialogModel(string peerName, long peerId, string lastMessage, Uri avatarUrl, long membercount):this(peerName, peerId, lastMessage, avatarUrl)
        {
            Online = new Online(membercount);
            
        }
        //for Users
        public DialogModel( string peerName,long peerId, string lastMessage, Uri avatarUrl, Online online):this(peerName, peerId, lastMessage, avatarUrl)
        {
            Online = new Online(online.IsOnline, online.WasOnline);
        }
        //for Groups
        

        public RelayCommand SendQuickReplyCommand
        {
            get =>
                (sendReply ?? (sendReply = new RelayCommand(SendQuickMessage, CanSendQuickMessage)));
            set => SetField(ref sendReply, value);

        }

        private void HandleOnlineChanged(User obj)
        {
            if (obj.Id == PeerId)
                Online.IsOnline = obj.Online;
        }

        private void HandleNewMessage(Message mes)
        {

            if (mes.PeerId == PeerId)
                LastMessage = mes.Text;
            //throw new NotImplementedException();
        }

        private void SendQuickMessage()
        {
            API.VkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = PeerId,
                Message = QuickReply,
                RandomId = new Random().Next(),

            });
            QuickReply = String.Empty;
            //LastMessage = QuickReply;
            //throw new NotImplementedException();
        }
        private bool CanSendQuickMessage()
        {
            return !String.IsNullOrEmpty(QuickReply);
        }
    }
}