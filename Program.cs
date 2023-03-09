using ManageUserPasswords.Controllers;
using ManageUserPasswords.Models;
using ManageUserPasswords.Repository;

/*
 * determine user ADMIN and default pass
 * create or read db file with users
 * check user pass
 */

string adminUsername = "ADMIN";
ReadUsernamePassword readBlock = new ReadUsernamePassword();
string defaultPassword = readBlock.HashPassword("password123");

/*
 * create or read db file with users
 */
string filePath = @"myUserList.txt";
string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
string fullPath = Path.Combine(projectDir, filePath);
TextRepository repo = new TextRepository();
List<User> registeredUsers = new List<User>();

if (File.Exists(fullPath))
{
    try
    {
        registeredUsers = repo.ReadListFromFile<User>(fullPath);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    }
}
else
{
    //create new file
    registeredUsers.Add(new User("ADMIN", defaultPassword));
    try
    {
        repo.SaveListToFile(registeredUsers, fullPath);
    }
    catch (Exception ex) 
    {
        Console.WriteLine();
        Environment.Exit(1);
    }
}

// Prompt the user for their username and password
Console.Write("Enter your username: ");
string username = Console.ReadLine();

User currentUser = registeredUsers.FirstOrDefault(x=>x.Username == username.Trim());

string findUserPass = currentUser.Password; //registeredUsers.Where(x => x.Username == username).Select(x => x.Password).FirstOrDefault();
if (findUserPass != null)
{
    bool blocked = registeredUsers.Where(x => x.Username == username).Select(x => x.IsBlocked).FirstOrDefault();
    if (blocked)
    {
        Console.WriteLine("User blocked! Write to admin");
        Environment.Exit(1);
    }
}
else
{
    // user not exists
    Console.WriteLine("User not exist! Write to admin");
    Environment.Exit(1);
}
/*
 * input and check password
 */
ReadUsernamePassword readerChecker = new ReadUsernamePassword();
readerChecker.CheckPassword(findUserPass);

// Check if the user is the administrator
if (currentUser.Username.ToUpper() == adminUsername)
{
    Console.WriteLine("Welcome, administrator!");

    // Enter administrator mode
    bool isAdminMode = true;
    int userIndex = 0;
    while (isAdminMode)
    {
        // Display the administrator menu
        Console.WriteLine("Administrator Mode:");
        Console.WriteLine("1. Change password");
        Console.WriteLine("2. View list of registered users");
        Console.WriteLine("3. Add user");
        Console.WriteLine("4. Exit");

        // Prompt the user for their choice
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        // Process the user's choice
        switch (choice)
        {
            case "1":

                // Change the administrator password
                readerChecker.ChangePassword(currentUser);

                break;

            case "2":
                // View the list of registered users
                Console.WriteLine("Registered Users:");
                Console.WriteLine("==================");
                foreach (User user in registeredUsers)
                {
                    Console.WriteLine("{0} - Password: {1} - Blocked: {2} - Restriction: {3}",
                        user.Username, user.Password, user.IsBlocked, user.RestrictedPassword);
                }
                Console.WriteLine("==================");

                // Prompt the user to move to the beginning or end of the list
                Console.WriteLine("Press B to move to the beginning of the list.");
                Console.WriteLine("Press E to move to the end of the list.");
                Console.WriteLine("Press any other key to return to the administrator menu.");
                string moveChoice = Console.ReadLine();
                switch (moveChoice.ToUpper())
                {
                    case "B":
                        userIndex = 0;
                        break;
                    case "E":
                        userIndex = registeredUsers.Count - 1;
                        break;
                }
                break;
            case "3":
                //add new user

                break;
            case "4":
                // Exit administrator mode
                isAdminMode = false;
                break;

            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}
else
{
    //current user can change the password
    // Display the user menu
    Console.WriteLine("User Mode:");
    Console.WriteLine("1. Change password");
    Console.WriteLine("2. Exit");

    // Prompt the user for their choice
    Console.Write("Enter your choice: ");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            // Change the user password
            readerChecker.ChangePassword(currentUser);
            break;
        case "2":

            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
            //Console.WriteLine("Authentication failed. Please try again.");
    
// Wait for user input before closing the console window