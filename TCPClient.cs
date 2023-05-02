using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace Message_Sender
{
    class TCPClient
    {
        private static Socket server;
        private static CancellationTokenSource SofiaAlekseevnaTheBest;
        public static List<Socket> clients = new List<Socket>();
        private static DialogWindow dialogWindow;
        public static void Launch(DialogWindow dialogWindow1, string IP, string name)
        {
            SofiaAlekseevnaTheBest = new CancellationTokenSource();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ConnectAsync(IP, 8888);
            RecieveMessage();
            dialogWindow = dialogWindow1;
            SendMessage($"/name {name}", "");
        }
        private static async Task RecieveMessage()
        {
            while (!SofiaAlekseevnaTheBest.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                
                if (message == "/clearlist")
                {
                    dialogWindow.Names.Clear();
                }
                else if (message.Split(' ')[0] == "/list")
                {
                    dialogWindow.Names.Add(message.Split(' ')[1]);
                    dialogWindow.ListBoxer.ItemsSource = null;
                    dialogWindow.ListBoxer.ItemsSource = dialogWindow.Names;
                }
                else if (message == "/disconnect")
                {
                    MessageBox.Show("Чат завершен.");
                    return;
                }
                else
                {

                }
                
                dialogWindow.AddMessage($"{DateTime.Now}, Сообщение от:[{message.Split('/')[1]}] {message.Split('/')[0]}");
            }
        }
        public static async Task SendMessage(string message, string name)
        {
            if (message[0]!='/')
            {
                message += $"/{name}";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(bytes, SocketFlags.None);
        }
    }
}
