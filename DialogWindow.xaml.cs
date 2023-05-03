using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Message_Sender
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public bool IsServer;
        public string AdmName;
        public string ClientName;
        public List<string> Names;
        public List<string> LogsList;
        public DialogWindow(string name, string ip, bool IsCreated)
        {
            InitializeComponent();
            IsServer = IsCreated;
            Names = new List<string>();
            LogsList = new List<string>();
            if (IsServer)
            {
                AdmName = name;
                ClientName = "/NotClient";
                Names.Add(name);
                LogsList.Add($"{DateTime.Now} - Чат создан");
                TCPServer.Launch(this);
            }
            else
            {
                AdmName = "/NotAdmin";
                ClientName = name;
                Names.Add(name);
                Logs.Height = 0;
                TCPClient.Launch(this, ip, name);
            }
            ListBoxer.ItemsSource = Names;
        }
        public void AddMessage(string message)
        {
            MessageListBox.Items.Add(message);
        }
        public void ClosingWindow(object sender, CancelEventArgs e)
        {
            if (IsServer)
            {
                TCPServer.SofiaAlekseevnaTheBest.Cancel();
            }
            else
            {
                TCPClient.SofiaAlekseevnaTheBest.Cancel();
            }
            Thread.Sleep(500);
            MainWindow window = new MainWindow();
            window.Show();
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (IsServer)
            {
                if (Message.Text == "/disconnect")
                {
                    TCPServer.SofiaAlekseevnaTheBest.Cancel();
                    Thread.Sleep(500);
                    MainWindow window = new MainWindow();
                    window.Show();
                    Close();
                }
                else
                {

                TCPServer.Send(Message.Text);
                }
            }
            else
            {
                if (Message.Text == "/disconnect")
                {
                    Thread.Sleep(500);
                    MainWindow window = new MainWindow();
                    window.Show();
                    Close();
                }
                else
                {
                TCPClient.SendMessage(Message.Text, ClientName);

                }
            }
            Message.Text = "";
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            ListBoxer.ItemsSource = null;
            ListBoxer.ItemsSource = LogsList;
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            ListBoxer.ItemsSource = null;
            ListBoxer.ItemsSource = Names;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (IsServer)
            {
                TCPServer.SofiaAlekseevnaTheBest.Cancel(); //:(
            }
            else
            {
                TCPClient.SofiaAlekseevnaTheBest.Cancel();
            }
            Thread.Sleep(500);
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
