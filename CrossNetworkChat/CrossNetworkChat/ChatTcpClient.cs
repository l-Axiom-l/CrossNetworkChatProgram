using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CrossNetworkChat
{
    public class ChatTcpClient
    {
        public event EventHandler<MessageEvent> MessageReceived;

        TcpClient client = new TcpClient();
        Stream s;

        public void Connect()
        {
            client.Connect("10.0.0.185", 700);
            s = client.GetStream();
            new Thread(Receiver).Start();
        }

        public void Disconnect()
        {
            client.Close();
        }

        public void Send(string ReceiverIP, string Message)
        {
            new Thread(Connect).Start();
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(Message);
            s.Write(ba, 0, ba.Length);
        }

        async void Receiver()
        {
            while (true)
            {
                if (client.Available <= 0)
                    continue;

                if (client.Connected == false)
                    break;

                byte[] buffer = new byte[100];
                int count = await s.ReadAsync(buffer, 0, 100);
                char[] temp = new char[100];
                for (int i = 0; i < count; i++)
                {
                    temp[i] = Convert.ToChar(buffer[i]);
                }
                MessageEvent messageEvent = new MessageEvent();
                messageEvent.Message = new string(temp);
                MessageReceived?.Invoke(this, messageEvent);
                await s.FlushAsync();
            }

        }
    }
}

public class MessageEvent : EventArgs
{
    public string Message;
}
