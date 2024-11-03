using MongoDB.Bson;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;
using System.Windows;

namespace SocialNetwork_WPF
{
    public partial class OtherPostsPage : Window
    {
        private readonly IPostDal _postDal;
        private readonly ObjectId _currentUserId;

        public OtherPostsPage(IPostDal postDal, ObjectId currentUserId)
        {
            InitializeComponent();
            _postDal = postDal;
            _currentUserId = currentUserId;
            LoadOtherPosts();
        }

        private async void LoadOtherPosts()
        {
            var posts = await _postDal.GetAllAsync();
            OtherPostsListBox.ItemsSource = posts.Where(post => post.UserId != _currentUserId).ToList();
        }

        private void OtherPostsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OtherPostsListBox.SelectedItem is PostDto selectedPost)
            {
                CommentTextBox.IsEnabled = true;
                AddCommentButton.IsEnabled = true;
                LikeButton.IsEnabled = true;
            }
        }

        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (OtherPostsListBox.SelectedItem is PostDto selectedPost && !string.IsNullOrEmpty(CommentTextBox.Text))
            {
                var comment = new CommentDto
                {
                    UserId = _currentUserId,
                    Content = CommentTextBox.Text,
                    CreatedAt = DateTime.UtcNow
                };

                await _postDal.AddCommentAsync(selectedPost.Id, comment);
                CommentTextBox.Clear();
                LoadOtherPosts();
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            if (OtherPostsListBox.SelectedItem is PostDto selectedPost)
            {
                var reaction = new ReactionDto
                {
                    UserId = _currentUserId,
                    ReactionType = "like",
                    CreatedAt = DateTime.UtcNow
                };

                await _postDal.AddReactionAsync(selectedPost.Id, reaction);
                LoadOtherPosts();
            }
        }
    }
}