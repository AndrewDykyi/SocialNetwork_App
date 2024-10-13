using System.Windows;
using System.Windows.Media;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork_App.DAL.Concrete;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;

namespace SocialNetwork_WPF
{
    public partial class MainWindow : Window
    {
        private readonly PostDal postDal;
        private readonly string userId = "6f143e8410bcfc3d9a76beb";
        private readonly IUserDal _userDal;

        public MainWindow(IUserDal userDal)
        {
            InitializeComponent();
            _userDal = userDal;
        }

        public MainWindow()
        {
            InitializeComponent();

            var client = new MongoClient("mongodb+srv://andriidyky:I4Ntr4eI0Lo74Xai@andreww.lg6vc.mongodb.net/?retryWrites=true&w=majority&appName=Andreww");
            var database = client.GetDatabase("social_network");

            postDal = new PostDal(database);

            postIdTextBox.Text = "Введіть ID поста";
            postIdTextBox.Foreground = Brushes.Gray;

            reactionTypeTextBox.Text = "Введіть тип реакції";
            reactionTypeTextBox.Foreground = Brushes.Gray;

            commentTextBox.Text = "Введіть коментар";
            commentTextBox.Foreground = Brushes.Gray;
        }

        private async void ShowComments_Click(object sender, RoutedEventArgs e)
        {
            var postIdText = postIdTextBox.Text;
            if (!ObjectId.TryParse(postIdText, out ObjectId postId))
            {
                MessageBox.Show("Неправильний формат ID поста.");
                return; 
            }

            var comments = await postDal.GetCommentsAsync(postId);
            MessageBox.Show($"Кількість коментарів: {comments.Count()}");
        }

        private async void ShowLikes_Click(object sender, RoutedEventArgs e)
        {
            var postIdText = postIdTextBox.Text;

            if (!ObjectId.TryParse(postIdText, out ObjectId postId))
            {
                MessageBox.Show("Неправильний формат ID поста.");
                return;
            }

            var reactions = await postDal.GetReactionsAsync(postId);
            MessageBox.Show($"Кількість лайків: {reactions.Count(r => r.ReactionType == "like")}");
        }

        private async void AddComment_Click(object sender, RoutedEventArgs e)
        {
            var postIdText = postIdTextBox.Text;
            if (!ObjectId.TryParse(postIdText, out ObjectId postId))
            {
                MessageBox.Show("Неправильний формат ID поста.");
                return;
            }

            var content = commentTextBox.Text;

            var comment = new CommentDto
            {
                UserId = new ObjectId(userId),
                Content = content,
                CreatedAt = DateTime.Now
            };

            await postDal.AddCommentAsync(postId, comment);
            MessageBox.Show("Коментар додано успішно.");
        }

        private async void AddReaction_Click(object sender, RoutedEventArgs e)
        {
            var postIdText = postIdTextBox.Text;
            var reactionType = reactionTypeTextBox.Text;

            if (!ObjectId.TryParse(postIdText, out ObjectId postId))
            {
                MessageBox.Show("Неправильний формат ID поста.");
                return;
            }

            var reaction = new ReactionDto
            {
                UserId = new ObjectId(userId),
                ReactionType = reactionType
            };

            await postDal.AddReactionAsync(postId, reaction);
            MessageBox.Show("Реакцію додано успішно.");
        }

        private void CommentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (commentTextBox.Text == "Введіть коментар")
            {
                commentTextBox.Text = string.Empty;
                commentTextBox.Foreground = Brushes.Black;
            }
        }

        private void CommentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(commentTextBox.Text))
            {
                commentTextBox.Text = "Введіть коментар";
                commentTextBox.Foreground = Brushes.Gray;
            }
        }

        private void PostIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (postIdTextBox.Text == "Введіть ID поста")
            {
                postIdTextBox.Text = string.Empty;
                postIdTextBox.Foreground = Brushes.Black;
            }
        }

        private void PostIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(postIdTextBox.Text))
            {
                postIdTextBox.Text = "Введіть ID поста";
                postIdTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ReactionTypeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (reactionTypeTextBox.Text == "Введіть тип реакції")
            {
                reactionTypeTextBox.Text = string.Empty;
                reactionTypeTextBox.Foreground = Brushes.Black;
            }
        }

        private void ReactionTypeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(reactionTypeTextBox.Text))
            {
                reactionTypeTextBox.Text = "Введіть тип реакції";
                reactionTypeTextBox.Foreground = Brushes.Gray;
            }
        }
        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var userId = new ObjectId("670bda080970d349be74ac84");
            var friendId = new ObjectId(friendIdTextBox.Text);
            await _userDal.AddFriendAsync(userId, friendId);
        }

        private async void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var userId = new ObjectId("ID_користувача");
            var friendId = new ObjectId(friendIdTextBox.Text);
            await _userDal.RemoveFriendAsync(userId, friendId);
        }

    }
}
