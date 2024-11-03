using System.Windows;
using MongoDB.Bson;
using SocialNetwork_WPF;

namespace Social_Network_WPF
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;

        public MainWindow(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userIdText = UserIdTextBox.Text;
            string password = PasswordBox.Password;

            if (ObjectId.TryParse(userIdText, out ObjectId userId))
            {
                var user = await _userService.GetByIdAsync(userId);

                if (user != null && user.Password == password)
                {
                    UserPage userPage = new UserPage(_userService, user.Id);
                    userPage.Show();
                    this.Close();
                }
                else
                {
                    ShowErrorMessage("Невірний ID користувача або пароль. Спробуйте ще раз.");
                }
            }
            else
            {
                ShowErrorMessage("Некоректний формат UserId.");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}