using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using ParsePhoneStarterProject.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ParsePhoneStarterProject {
    public partial class MainPage : PhoneApplicationPage {

        private static string MY_USER = "Tiago";
        private static string MY_COLOR = "#ff0000";

        public MainPage() {
            InitializeComponent();

            // Initialize parse
            ParseClient.Initialize("KAimpzFF8jSa1QSJb0NSnMI4IHkCy4ppxHfEDJh7", "piXJteRmT3173EwPS3psNjguPN0hdOCYOsOpguAY");

            ToListPicker.SelectionChanged += new SelectionChangedEventHandler(ToListPicker_SelectionChanged);
        }

        private void ToListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetMessages();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
            GetMessages();
        }

        private void SendMessage()
        {
            var chatMessage = new ParseObject("ChatMessage");
            chatMessage["color"] = MY_COLOR;
            chatMessage["message"] = Message.Text;
            chatMessage["user"] = MY_USER;
            chatMessage["to"] = ((TextBlock)ToListPicker.SelectedItem).Text;
            chatMessage.SaveAsync();
        }

        private void GetMessages()
        {
            List<ChatMessageModel> chatMessages = new List<ChatMessageModel>();

            var query = ((TextBlock)ToListPicker.SelectedItem).Text == "All" ?
                from chatMessage in ParseObject.GetQuery("ChatMessage")
                where chatMessage.Get<string>("to") == "All"
                orderby chatMessage.Get<DateTime>("createdAt") ascending
                select chatMessage
                :
                from chatMessage in ParseObject.GetQuery("ChatMessage")
                where (chatMessage.Get<string>("user") == MY_USER &&
                       chatMessage.Get<string>("to") == ((TextBlock)ToListPicker.SelectedItem).Text) ||
                      (chatMessage.Get<string>("user") == ((TextBlock)ToListPicker.SelectedItem).Text &&
                       chatMessage.Get<string>("to") == MY_USER)
                orderby chatMessage.Get<DateTime>("createdAt") ascending
                select chatMessage;

            query.FindAsync().ContinueWith(t => 
            {
                IEnumerable<ParseObject> results = t.Result;
                foreach (var chatMessage in results) 
                {
                    String color = chatMessage.Get<string>("color");
                    String message = chatMessage.Get<string>("message");
                    String user = chatMessage.Get<string>("user");
                    String to = chatMessage.Get<string>("to");
                    
                    chatMessages.Add(new ChatMessageModel(color, message, user, to));
                }    
            });

            ChatMessageList.ItemsSource = chatMessages;
        }
    }
}