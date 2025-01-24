using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для UserProfileWindow.xaml
    /// </summary>
    public partial class UserProfileWindow : Window
    {
        public UserProfile userProfile;
        public event Action<string> UserNameUpdated;
        public UserProfileWindow(UserProfile profile)
        {
            InitializeComponent();
            userProfile = profile;
            LoadUserProfile();
        }
        private void LoadUserProfile()
        {
            UserNameTextBox.Text = userProfile.UserName;
            AboutTextBox.Text = userProfile.About;
            StatusTextBlock.Text = userProfile.Status;

            if (File.Exists(userProfile.AvatarPath))
            {
                AvatarImage.Source = new BitmapImage(new Uri(userProfile.AvatarPath, UriKind.RelativeOrAbsolute));
            }
            
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if(openFileDialog.ShowDialog() == true)
            {
                userProfile.AvatarPath = openFileDialog.FileName;
                AvatarImage.Source = new BitmapImage(new Uri(userProfile.AvatarPath));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            userProfile.UserName = UserNameTextBox.Text;
            userProfile.About = AboutTextBox.Text;
            UserNameUpdated?.Invoke(userProfile.UserName);
            MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
}
