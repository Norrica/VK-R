using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet.Enums;

namespace VK_R
{
    internal class MessagesModel : PropertyChangedBase
    {
        private string text;
        private string peerName;
        private Uri avatar;
        private DateTime sentOn;
        private bool readState;

        public string Text { get => text; set => SetField(ref text, value); }
        public string PeerName { get => peerName; set => SetField(ref peerName, value); }
        public Uri Avatar { get => avatar; set => SetField(ref avatar, value); }
        public DateTime SentOn { get => sentOn; set => SetField(ref sentOn, value); }
        public bool ReadState { get => readState; set => SetField(ref readState, value); }

        public MessagesModel(string text, string peername, Uri avatar, DateTime sentOn)
        {
            Text = text;
            PeerName = peername;
            Avatar = avatar;
            SentOn = sentOn;
        }
    }
}