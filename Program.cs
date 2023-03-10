
//https://github.com/VetalRovniy/ManageUserPasswords

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
string defaultPassword = readBlock.HashPassword("1234");

/*
 * create or read db file with users
 */
string filePath = @"myUserList.txt";
string projectDir = Directory.GetCurrentDirectory();//Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
string fullPath = Path.Combine(projectDir, filePath);
TextRepository repo = new TextRepository();
List<User> registeredUsers = new List<User>();

//Console.WriteLine(projectDir);

if (File.Exists(fullPath))
{
    try
    {
        registeredUsers = repo.ReadListFromFile(fullPath);
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
if (currentUser == null)
{
    Console.WriteLine("User not exist!");
    Environment.Exit(1);    
}
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
Console.Write("Enter your password: ");
int maxCount = 3;


while (!readerChecker.CheckPassword(findUserPass) && maxCount>=0)
{
    Console.Write("Enter your password: ");
    maxCount--;
    if (maxCount <= 0)
     Environment.Exit(1);
    
}


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
        Console.WriteLine("4. Edit user");
        Console.WriteLine("5. Delete user");
        Console.WriteLine("6. Exit");

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
                Console.WriteLine("Enter new user name:");
                string newUsername = Console.ReadLine();
                User newUser = registeredUsers.FirstOrDefault(x => x.Username == newUsername.Trim());
                if (newUser != null)
                {
                    Console.WriteLine("User exist!");
                    // Console.WriteLine("Press any other key to return to the administrator menu.");
                    break;
                }


                registeredUsers.Add(new User(newUsername, readBlock.HashPassword("1234")));

                //Console.WriteLine("Enter new user password:");

                break;
            case "4":
                //edit user
                Console.WriteLine("Enter user name: ");
                string editedUsername = Console.ReadLine();
                User editedUser = registeredUsers.FirstOrDefault(x => x.Username == editedUsername.Trim());
                if (editedUser == null)
                {
                    Console.WriteLine("User not exist!");
                    //Console.WriteLine("Press any other key to return to the administrator menu.");
                    break;
                }

                Console.WriteLine("Choose edit attributes:");
                Console.WriteLine("1. Edit username");
                Console.WriteLine("2. Edit password");
                Console.WriteLine("3. Block user");
                Console.Write("Enter your choice: ");
                string editedChoise = Console.ReadLine();
                switch (editedChoise)
                {
                    case "1":
                        Console.WriteLine("Enter new user name:");
                        editedUser.Username = Console.ReadLine().Trim();

                        break;
                    case "2":
                        Console.WriteLine("Enter new user name:");
                        editedUser.Password = readBlock.HashPassword("1234");
                        break;
                    case "3":
                        Console.WriteLine("Block user? Y/N");
                        string editBlock = Console.ReadLine();
                        switch (editBlock)
                        {
                            case "Y":
                                editedUser.IsBlocked = true;
                                break;
                            case "N":
                                editedUser.IsBlocked = false;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }



                break;
            case "5":
                // Exit administrator mode
                Console.WriteLine("Enter user name for delete:");
                string delUsername = Console.ReadLine();
                User delUser = registeredUsers.FirstOrDefault(x => x.Username == delUsername.Trim());
                if (delUser == null)
                {
                    Console.WriteLine("User not exist!");
                    // Console.WriteLine("Press any other key to return to the administrator menu.");
                    break;
                }
                registeredUsers.Remove(delUser);
                Console.WriteLine($"User {delUsername} succefully deleted");
                break;
            case "6":
                // Exit administrator mode
                isAdminMode = false;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        //repo.SaveListToFile(registeredUsers, fullPath);
    }
}
else
{
    //current user can change the password
    // Display the user menu
    Console.WriteLine("User Mode:");
    Console.WriteLine("1. Change password");
    Console.WriteLine("2. Change user name");
    Console.WriteLine("3. Exit");

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
            Console.WriteLine("Enter new user name:");
            
            currentUser.Username = Console.ReadLine().Trim();

            break;
        case "3":

            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }

    //repo.SaveListToFile(registeredUsers, fullPath);
}
repo.SaveListToFile(registeredUsers, fullPath);
//Console.WriteLine("Authentication failed. Please try again.");

// Wait for user input before closing the console window