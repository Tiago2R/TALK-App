using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ParsePhoneStarterProject
{
    class ChatMessageModel
    {
        public Color Color { get; set; }
        public String Message { get; set; }
        public String User { get; set; }
        public String To { get; set; }

        public ChatMessageModel(String color, String message, String user, String to)
        {
            Color = ColorConverter.GetColorFromHex(color);
            Message = message;
            User = user + ":";
            To = to;
        }
    }
}
