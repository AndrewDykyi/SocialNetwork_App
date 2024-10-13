using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DAL.Concrete;
using SocialNetwork_App.DTO;
using MongoDB.Driver;
using MongoDB.Bson;

class Program
{
    private static IMongoDatabase database;

    static async Task Main(string[] args)
    {
        var client = new MongoClient("mongodb+srv://andriidyky:I4Ntr4eI0Lo74Xai@andreww.lg6vc.mongodb.net/?retryWrites=true&w=majority&appName=Andreww");
        database = client.GetDatabase("social_network");

        UserDal userDal = new UserDal(database);
        PostDal postDal = new PostDal(database);
        CommentDal commentDal = new CommentDal(database);

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n=== Social Network Application ===");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Show All Users");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("5. Add Post");
            Console.WriteLine("6. Show All Posts");
            Console.WriteLine("7. Add Comment");
            Console.WriteLine("8. Show All Comments");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an action: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddUser(userDal);
                    break;
                case "2":
                    await ShowAllUsers(userDal);
                    break;
                case "3":
                    await UpdateUser(userDal);
                    break;
                case "4":
                    await DeleteUser(userDal);
                    break;
                case "5":
                    await AddPost(postDal);
                    break;
                case "6":
                    await ShowAllPosts(postDal);
                    break;
                //case "7":
                //    await AddComment(commentDal);
                //    break;
                //case "8":
                //    await ShowAllComments(commentDal);
                //    break;
                case "9":
                    isRunning = false;
                    Console.WriteLine("Program terminated.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task AddUser(IUserDal userDal)
    {
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        Console.Write("Enter First Name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter Last Name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter Interests (comma-separated): ");
        string interestsInput = Console.ReadLine();
        List<string> interests = new List<string>(interestsInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

        UserDto newUser = new UserDto
        {
            Id = ObjectId.GenerateNewId(),
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            Interests = interests
        };

        await userDal.InsertAsync(newUser);
        Console.WriteLine("User added successfully.");
    }


    static async Task ShowAllUsers(IUserDal userDal)
    {
        var users = await userDal.GetAllAsync();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, First Name: {user.FirstName}, Last Name: {user.LastName}, Email: {user.Email}");
        }
    }

    static async Task UpdateUser(IUserDal userDal)
    {
        Console.Write("Enter the ID of the user to update: ");
        string idString = Console.ReadLine();

        if (!ObjectId.TryParse(idString, out ObjectId id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var user = await userDal.GetByIdAsync(id);
        if (user == null)
        {
            Console.WriteLine("User with that ID not found.");
            return;
        }

        Console.Write("Enter new Email (press Enter to keep unchanged): ");
        string newEmail = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newEmail))
        {
            user.Email = newEmail;
        }

        Console.Write("Enter new Password (press Enter to keep unchanged): ");
        string newPassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            user.Password = newPassword;
        }

        Console.Write("Enter new First Name (press Enter to keep unchanged): ");
        string newFirstName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newFirstName))
        {
            user.FirstName = newFirstName;
        }

        Console.Write("Enter new Last Name (press Enter to keep unchanged): ");
        string newLastName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newLastName))
        {
            user.LastName = newLastName;
        }

        Console.Write("Enter new Interests (comma-separated) (press Enter to keep unchanged): ");
        string interestsInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(interestsInput))
        {
            user.Interests = new List<string>(interestsInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        await userDal.UpdateAsync(id, user);
        Console.WriteLine("User updated successfully.");
    }

    static async Task DeleteUser(IUserDal userDal)
    {
        Console.Write("Enter the ID of the user to delete: ");
        string idString = Console.ReadLine();

        if (!ObjectId.TryParse(idString, out ObjectId id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        await userDal.DeleteAsync(id);
        Console.WriteLine("User deleted successfully.");
    }

    static async Task AddPost(PostDal postDal)
    {
        Console.Write("Enter User ID: ");
        string userId = Console.ReadLine();

        if (!ObjectId.TryParse(userId, out ObjectId parsedUserId))
        {
            Console.WriteLine("Invalid User ID format.");
            return;
        }

        Console.Write("Enter Post Content: ");
        string content = Console.ReadLine();

        PostDto newPost = new PostDto
        {
            Id = ObjectId.GenerateNewId(),
            UserId = parsedUserId,
            Content = content,
            CreatedAt = DateTime.Now
        };

        await postDal.InsertAsync(newPost);
        Console.WriteLine("Post added successfully.");
    }


    static async Task ShowAllPosts(IPostDal postDal)
    {
        var posts = await postDal.GetAllAsync();
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, User ID: {post.UserId}, Content: {post.Content}, Created At: {post.CreatedAt}");
        }

        if (!posts.Any())
        {
            Console.WriteLine("No posts to display.");
        }
    }
}