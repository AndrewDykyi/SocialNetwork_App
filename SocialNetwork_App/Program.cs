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

        var neo4JConnection = new Neo4JConnection("neo4j+s://728d20c8.databases.neo4j.io", "neo4j", "0G_eoDfGWO6bi7eh4OHFc2mvDSZz3NoFjEbBs_uK3Pk");
        INeo4JUserDal neo4jUserDal = new Neo4JUserDal(neo4JConnection);

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
            Console.WriteLine("7. Add Friendship");
            Console.WriteLine("8. Delete Friendship");
            Console.WriteLine("9. Check if Users are Connected");
            Console.WriteLine("10. Get Distance Between Users");
            Console.WriteLine("11. Exit");
            Console.Write("Choose an action: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddUser(userDal, neo4jUserDal);
                    break;
                case "2":
                    await ShowAllUsers(userDal);
                    break;
                case "3":
                    await UpdateUser(userDal, neo4jUserDal);
                    break;
                case "4":
                    await DeleteUser(userDal, neo4jUserDal);
                    break;
                case "5":
                    await AddPost(postDal);
                    break;
                case "6":
                    await ShowAllPosts(postDal);
                    break;
                case "7":
                    await AddFriendship(neo4jUserDal);
                    break;
                case "8":
                    await DeleteFriendship(neo4jUserDal);
                    break;
                case "9":
                    await CheckIfUsersConnected(neo4jUserDal);
                    break;
                case "10":
                    await GetDistanceBetweenUsers(neo4jUserDal);
                    break;
                case "11":
                    isRunning = false;
                    Console.WriteLine("Program terminated.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task AddUser(IUserDal userDal, INeo4JUserDal neo4jUserDal)
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
        Console.WriteLine("User added successfully to MongoDB.");

        await neo4jUserDal.CreateUserAsync(newUser.Id.ToString(), newUser.FirstName);
        Console.WriteLine("User added successfully to Neo4j.");
    }

    static async Task ShowAllUsers(IUserDal userDal)
    {
        var users = await userDal.GetAllAsync();
        if (users.Any())
        {
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, First Name: {user.FirstName}, Last Name: {user.LastName}, Email: {user.Email}");
            }
        }
        else
        {
            Console.WriteLine("No users found.");
        }
    }

    static async Task UpdateUser(IUserDal userDal, INeo4JUserDal neo4jUserDal)
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
            await neo4jUserDal.UpdateUserNameAsync(user.Id.ToString(), user.FirstName);
            Console.WriteLine("User's name updated in Neo4j.");
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
        Console.WriteLine("User updated successfully in MongoDB.");
    }

    static async Task DeleteUser(IUserDal userDal, INeo4JUserDal neo4jUserDal)
    {
        Console.Write("Enter the ID of the user to delete: ");
        string idString = Console.ReadLine();

        if (!ObjectId.TryParse(idString, out ObjectId id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        await userDal.DeleteAsync(id);
        Console.WriteLine("User deleted successfully from MongoDB.");

        await neo4jUserDal.DeleteUserAsync(id.ToString());
        Console.WriteLine("User deleted successfully from Neo4j.");
    }

    static async Task AddPost(IPostDal postDal)
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
            CreatedAt = DateTime.UtcNow
        };

        await postDal.InsertAsync(newPost);
        Console.WriteLine("Post added successfully.");
    }

    static async Task ShowAllPosts(IPostDal postDal)
    {
        var posts = await postDal.GetAllAsync();
        if (posts.Any())
        {
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}, User ID: {post.UserId}, Content: {post.Content}, Created At: {post.CreatedAt}");
            }
        }
        else
        {
            Console.WriteLine("No posts to display.");
        }
    }

    static async Task AddFriendship(INeo4JUserDal neo4jUserDal)
    {
        Console.Write("Enter User ID 1: ");
        string userId1 = Console.ReadLine();

        Console.Write("Enter User ID 2: ");
        string userId2 = Console.ReadLine();

        await neo4jUserDal.CreateRelationshipAsync(userId1, userId2, "FRIEND");
        Console.WriteLine("Friendship added successfully in Neo4j.");
    }

    static async Task DeleteFriendship(INeo4JUserDal neo4jUserDal)
    {
        Console.Write("Enter User ID 1: ");
        string userId1 = Console.ReadLine();

        Console.Write("Enter User ID 2: ");
        string userId2 = Console.ReadLine();

        await neo4jUserDal.DeleteRelationshipAsync(userId1, userId2, "FRIEND");
        Console.WriteLine("Friendship deleted successfully in Neo4j.");
    }

    static async Task CheckIfUsersConnected(INeo4JUserDal neo4jUserDal)
    {
        Console.Write("Enter User ID 1: ");
        string userId1 = Console.ReadLine();

        Console.Write("Enter User ID 2: ");
        string userId2 = Console.ReadLine();

        bool isConnected = await neo4jUserDal.AreUsersConnectedAsync(userId1, userId2);
        Console.WriteLine(isConnected ? "Users are connected." : "Users are not connected.");
    }
    static async Task GetDistanceBetweenUsers(INeo4JUserDal neo4jUserDal)
    {
        Console.Write("Enter User ID 1: ");
        string userId1 = Console.ReadLine();

        Console.Write("Enter User ID 2: ");
        string userId2 = Console.ReadLine();

        int distance = await neo4jUserDal.GetDistanceBetweenUsersAsync(userId1, userId2);
        if (distance != -1)
        {
            Console.WriteLine($"The distance between the users is: {distance}");
        }
        else
        {
            Console.WriteLine("The users are not connected.");
        }
    }
}