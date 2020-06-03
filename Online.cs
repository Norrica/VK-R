using System;
using System.Windows.Controls;

namespace VK_R
{
    internal class Online : PropertyChangedBase
    {
        //add displayedDevice  if possible
        private bool? isOnline;
        private DateTime wasOnline = new DateTime(2000, 1, 1);
        private string displayedText;
        public Online(long membercount) 
        {
            DisplayedText = membercount.ToString();
        }
        public Online() { }
        public Online(bool? isOnline, DateTime? lastSeen)
        {
            IsOnline = isOnline;
            bool online = IsOnline.HasValue ? IsOnline.Value : false;
            if (online)
            {
                DisplayedText = "online";
            }
            else
            {
                if (lastSeen.HasValue)
                {
                    WasOnline = lastSeen.Value.ToLocalTime();
                    DisplayedText = WasOnline.ToString();
                }
            }
        }

        public bool? IsOnline { get => isOnline; set => SetField(ref isOnline, value); }
        public DateTime WasOnline { get => wasOnline; set => SetField(ref wasOnline, value); }
        public string DisplayedText { get => displayedText; set => SetField(ref displayedText , value); }
    }
}