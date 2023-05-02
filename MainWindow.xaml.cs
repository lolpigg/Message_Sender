using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Message_Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Regex name = new Regex("^[a-zA-Z0-9]+$");
        Regex ip = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!name.IsMatch(Name.Text))
            {
                MessageBox.Show("Имя не соответствует требованиям.");
                return;
            }
            DialogWindow window = new DialogWindow(Name.Text, IP.Text, true);
            window.Show();
            Close();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            if (!name.IsMatch(Name.Text))
            {
                MessageBox.Show("Имя не соответствует требованиям.");
                return;
            }
            if (!ip.IsMatch(IP.Text))
            {
                MessageBox.Show("IP введено в неверном формате!");
                return;
            }
            try
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(IP.Text, 8888);
            }
            catch (Exception)
            {
                MessageBox.Show("Сервер не найден!");
                return;
            }
            DialogWindow window = new DialogWindow(Name.Text, IP.Text, false);
            window.Show();
            Close();
        }
    }
}
