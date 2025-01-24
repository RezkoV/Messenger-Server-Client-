using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net.Sockets;
using System.Diagnostics.PerformanceData;

namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string UserName = "Вы";
        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();


        }

        private void ConnectToServer()
        {
            client = new TcpClient("127.0.0.1", 8888);
            stream = client.GetStream();
            StartReceivingMessages();
            LanguageSelector.SelectedIndex = 0;
        }

        private async void StartReceivingMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Dispatcher.Invoke(() => MessagesList.Items.Add(message));
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = new UserProfile();
            string message = MessageInput.Text;
            if (!string.IsNullOrEmpty(message))
            {
                string formattedMessage = $"{userProfile.UserName}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                MessagesList.Items.Add(formattedMessage);
                MessageInput.Clear();
            }
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LanguageSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Русский")
                {
                    UserName = "Вы";
                    MessageInput.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ru-RU");
                }
                else
                {
                    UserName = "You";
                    MessageInput.Language = System.Windows.Markup.XmlLanguage.GetLanguage("en-US");
                }
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = new UserProfile();
            UserProfileWindow profileWindow = new UserProfileWindow(userProfile);
            profileWindow.ShowDialog();
        }
    }
}
