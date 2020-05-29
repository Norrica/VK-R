using System;
using System.Windows.Controls;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Image = System.Windows.Controls.Image;

namespace VK_R
{
    internal class DialogModel : PropertyChangedBase
    {
        ApiBase API = ApiBase.GetInstance();
        private string peerName = "ss";
        private int peerId;//maybe not necessary
        private string lastMessage = "lm";
        private Image profilePicture;
        private Image lastdevice;
        private bool isOnline;
        private DateTime wasOnline = new DateTime(2000, 1, 1);
        private string quickReply;
        private Command sendReply;

        public string PeerName { get => peerName; set => peerName = value; }
        public int PeerId { get => peerId; set => peerId = value; }
        public string LastMessage { get => lastMessage; set => lastMessage = value; }
        public Image ProfilePicture { get => profilePicture; set => profilePicture = value; }
        public Image Lastdevice { get => lastdevice; set => lastdevice = value; }
        public bool IsOnline { get => isOnline; set => isOnline = value; }
        public DateTime WasOnline { get => wasOnline; set => wasOnline = value; }
        public string QuickReply { get => quickReply; set => SetField(ref quickReply, value); }

        public DialogModel() { }
        public DialogModel(string peerName, string lastMessage)
        {
            PeerName = peerName;
            LastMessage = lastMessage;
        }

        //public Command SendQuickReplyCommand
        //{
        //    get =>
        //        (sendReply ?? (sendReply = new Command(SendQuickMessage, CanSendQuickMessage)));
        //    set => SetField(ref sendReply, value);

        //}
        private void SendQuickMessage()
        {
            API.VkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = PeerId,
                Message = QuickReply,
                RandomId = new Random().Next(),

            }) ;
            throw new NotImplementedException();
        }
        private bool CanSendQuickMessage()
        {
            return !String.IsNullOrEmpty(QuickReply);
            //throw new NotImplementedException();
        }
    }
}