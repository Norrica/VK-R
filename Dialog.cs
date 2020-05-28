using System;
using System.Windows.Controls;
using VkNet.Model;
using Image = System.Windows.Controls.Image;

namespace VK_R
{
    internal class DialogModel:PropertyChangedBase
    {
        private string peerName="ss";
        private int id;//maybe not necessary
        private string lastMessage="lm";
        private Image profilePicture;
        private Image lastdevice;
        private bool isOnline;
        private DateTime wasOnline=new DateTime(2000,1,1);
        #warning implement commands, dumb-dumb
        private Command sendReply;

        public string PeerName { get => peerName; set => peerName = value; }
        public int Id { get => id; set => id = value; }
        public string LastMessage { get => lastMessage; set => lastMessage = value; }
        public Image ProfilePicture { get => profilePicture; set => profilePicture = value; }
        public Image Lastdevice { get => lastdevice; set => lastdevice = value; }
        public bool IsOnline { get => isOnline; set => isOnline = value; }
        public DateTime WasOnline { get => wasOnline; set => wasOnline = value; }

        public DialogModel() { }
        public DialogModel(string peername, string lastMessage,bool isOnline)
        {

        }
        
    }
}