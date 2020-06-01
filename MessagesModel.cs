using System;
using System.Windows.Controls;

namespace VK_R
{
    internal class MessagesModel : PropertyChangedBase
    {
        private string text;
        private string peerName;
        private Image avatar;
        private DateTime sentOn;

        public string Text { get => text; set => SetField(ref text, value); }
        public string PeerName { get => peerName; set => SetField(ref peerName, value); }
        public Image Avatar { get => avatar; set => SetField(ref avatar, value); }
        public DateTime SentOn { get => sentOn; set => SetField(ref sentOn, value); }
    }
}