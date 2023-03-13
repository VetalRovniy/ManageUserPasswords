using ManageUserPasswords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManageUserPasswords.Controllers
{
    public class ReadUsernamePassword
    {
        public bool CheckPassword(string storedPasswordHash)
        {
            //string storedPasswordHash = HashPassword("password123"); // Store hashed password
            //Console.Write("Enter your password: ");
            string inputPassword = ReplacePassPhrase();


            if (VerifyPassword(inputPassword, storedPasswordHash))
            {
                //Console.WriteLine("\nPassword is correct!");
                return true;
            }
            else
            {
                Console.WriteLine("\nPassword is incorrect!");
                return false;
            }
        }
        public string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string hashedPassword = Convert.ToBase64String(hashBytes);
                return hashedPassword;
            }
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }

        }

        public void ChangePassword(User currentUser)
        {
            // Change the administrator password
            Console.Write("Enter the old password: ");
            string oldPassword = ReplacePassPhrase();//Console.ReadLine();
            


            if (VerifyPassword(oldPassword, currentUser.Password))
            {
                Console.Write("\nEnter the new password: ");
                string newPassword = ReplacePassPhrase();//Console.ReadLine();
                Console.Write("\nConfirm the new password: ");
                string confirmPassword = ReplacePassPhrase();
                if (newPassword == confirmPassword)
                {
                    currentUser.Password = HashPassword(newPassword);
                    Console.WriteLine("\nPassword changed successfully.");
                   
                }
                else
                {
                    Console.WriteLine("\nPasswords do not match. Password not changed.");
                    
                }
            }
            else
            {
                Console.WriteLine("\nInvalid password. Password not changed.");
                
            }
            
        }

        private string ReplacePassPhrase()
        {
            string inputPassword = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true); // true parameter hides the key being pressed

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    /*here we can check input characrers
                     * 6.	Наявність букв і знаків арифметичних операцій.
                     */
                    if (!char.IsLetter(key.KeyChar)&&(!char.IsSymbol(key.KeyChar)) )
                    {
                        inputPassword += key.KeyChar;
                        Console.Write("*"); // replace character with asterisk
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && inputPassword.Length > 0)
                {
                    inputPassword = inputPassword.Substring(0, inputPassword.Length - 1);
                    Console.Write("\b \b"); // erase the last character and replace with space
                }
            } while (key.Key != ConsoleKey.Enter);

            return inputPassword;
        }
    
    }
}
