using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossNetworkChat
{
    public partial class MainPage : ContentPage
    {
        ChatTcpClient client;
        public MainPage()
        {
            InitializeComponent();
            Contacts.Clicked += GotoContacts;
            client = new ChatTcpClient();
            Send.Clicked += SendMessage;
            client.MessageReceived += MessageReceived;
        }

        void SendMessage(object s, EventArgs e)
        {
            client.Send("10.10.10.10", Input.Text);
        }

        async void GotoContacts(object s, EventArgs e)
        {
            await Navigation.PushAsync(new Contacts(this), true);
        }

        void MessageReceived(object s, MessageEvent message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Chat.Text += "\n" + message.Message;
            });

        }
    }
}
