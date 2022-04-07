using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ChatServer
{
    internal class Program
    {
        static IPAddress address;
        static TcpListener listener;

        static void Main(string[] args)
        {
        LOOP:
            Console.WriteLine("Type your Command");
            switch(Console.ReadLine())
            {
                default: goto LOOP;

                case "StartServer":
                    new Thread(StartServer).Start();
                    break;

                case "StopServer":
                    StopServer();
                    break;

                case "StartClient":
                    StartClient();
                    break;
            }
            goto LOOP;
        }

        static void StartServer()
        {
            address = IPAddress.Parse("10.0.0.185");
            listener = new TcpListener(address, 700);
            listener.Start();
            Console.WriteLine("The server is running at port 8001...");
            Console.WriteLine("The local End point is  :" +
                              listener.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");
            LOOP:
            Socket s = listener.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
            byte[] b = new byte[100];
            int k = s.Receive(b);
            Console.WriteLine("Received....");
            for(int i = 0; i < k; i++)
            {
                Console.Write(Convert.ToChar(b[i]));
            }

            ASCIIEncoding asen = new ASCIIEncoding();
            s.Send(asen.GetBytes("ErrorCode:0"));
            s.Close();
            goto LOOP;
        }

        static void StopServer()
        {
            listener.Stop();
        }

        static void StartClient()
        {
            TcpClient tcpclnt = new TcpClient();
            Console.WriteLine("Connecting.....");

            tcpclnt.Connect("10.0.0.185", 700);
            // use the ipaddress as in the server program

            Console.WriteLine("Connected");
            Console.Write("Enter the string to be transmitted : ");

            String str = Console.ReadLine();
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            Console.WriteLine("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(bb[i]));

            tcpclnt.Close();
        }
    }
}
