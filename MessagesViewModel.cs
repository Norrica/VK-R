using System;
using System.Collections.ObjectModel;

namespace VK_R
{
    internal class MessagesViewModel : PropertyChangedBase
    {
        private ObservableCollection<MessagesModel> messages = new ObservableCollection<MessagesModel>();
        public MessagesViewModel()
        {
            GetMessages();
        }

        private void GetMessages()
        {
            throw new NotImplementedException();
        }

        internal ObservableCollection<MessagesModel> Messages { get => messages; set => SetField(ref messages, value); }
    }
}