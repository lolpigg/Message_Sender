using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Message_Sender
{
    public class TCPServer
    {
        public static Socket socket;
        private static IPEndPoint ipPoint;
        public static CancellationTokenSource SofiaAlekseevnaTheBest;
        public  static List<Socket> clients = new List<Socket>();
        private static DialogWindow dialogWindow;
        public static void Launch(DialogWindow dialogWindow1)
        {
            SofiaAlekseevnaTheBest = new CancellationTokenSource();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket.Bind(ipPoint);
            socket.Listen(1000);
            ListenClients();
            dialogWindow = dialogWindow1;
        }
        private static async Task ListenClients()
        {
            while (!SofiaAlekseevnaTheBest.IsCancellationRequested)
            {
                Socket client = await socket.AcceptAsync();
                clients.Add(client);
                RecieveMessage(client);
            }
        }
        public static async Task RecieveMessage(Socket client)
        {
            while (!SofiaAlekseevnaTheBest.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                if (message.Split(' ')[0] == "/name")
                {
                    dialogWindow.Names.Add(message.Split(' ')[1]);
                    dialogWindow.LogsList.Add($"{DateTime.Now} - {message.Split(' ')[1]} присоединился к чату.");
                    
                }
                else
                {
                    dialogWindow.AddMessage($"{DateTime.Now}, Сообщение от:[{message.Split('/')[1]}] {message.Split('/')[0]}");
                    foreach (Socket item in clients)
                    {
                        SendMessage(item, message);
                    }
                }
            }
        }
        private static async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);
        }
        public static async Task Send(string message)
        {
            if (message != "/disconnect")
            {
                dialogWindow.AddMessage($"{DateTime.Now}, Сообщение от:[{dialogWindow.AdmName}] {message}");
                message += $"/{dialogWindow.AdmName}";
            }
            foreach (Socket item in clients)
            {
                SendMessage(item, message);
            }
        }
    }
}
