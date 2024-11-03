using MongoDB.Bson;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;
using System.Collections.Generic;
using System.Windows;

namespace SocialNetwork_WPF
{
    public partial class UserPage : Window
    {
        private readonly IPostDal _postDal;
        private readonly ObjectId _currentUserId;

        public UserPage(IPostDal postDal, ObjectId currentUserId)
        {
            InitializeComponent();
            _postDal = postDal;
            _currentUserId = currentUserId;
            LoadUserPosts();
        }

        private async void LoadUserPosts()
        {
            var posts = await _postDal.GetAllAsync();
            PostsListBox.ItemsSource = posts.Where(post => post.UserId == _currentUserId).ToList();
        }

        private void PostsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PostsListBox.SelectedItem is PostDto selectedPost)
            {
                CommentTextBox.IsEnabled = true;
                AddCommentButton.IsEnabled = true;
                LikeButton.IsEnabled = true;
            }
        }

        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (PostsListBox.SelectedItem is PostDto selectedPost && !string.IsNullOrEmpty(CommentTextBox.Text))
            {
                var comment = new CommentDto
                {
                    UserId = _currentUserId,
                    Content = CommentTextBox.Text,
                    CreatedAt = DateTime.UtcNow
                };

                await _postDal.AddCommentAsync(selectedPost.Id, comment);
                CommentTextBox.Clear();
                LoadUserPosts();
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            if (PostsListBox.SelectedItem is PostDto selectedPost)
            {
                var reaction = new ReactionDto
                {
                    UserId = _currentUserId,
                    ReactionType = "like",
                    CreatedAt = DateTime.UtcNow
                };

                await _postDal.AddReactionAsync(selectedPost.Id, reaction);
                LoadUserPosts();
            }
        }

        private void ViewOtherPostsButton_Click(object sender, RoutedEventArgs e)
        {
            OtherPostsPage otherPostsPage = new OtherPostsPage(_postDal, _currentUserId);
            otherPostsPage.Show();
        }
    }
}