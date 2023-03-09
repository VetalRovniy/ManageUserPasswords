using ManageUserPasswords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageUserPasswords.Repository
{
    public class TextRepository
    {
        public bool SaveListToFile(List<User> list, string filePath)
        {
            try
            {
                // Convert the list to a string
                string data = string.Join(Environment.NewLine, list);

                // Write the string to the text file
                File.WriteAllText(filePath, data);
                Console.WriteLine("Save succefully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Some error:{ex.Message}");
                return false;
            }
        }

        public List<User> ReadListFromFile<User>(string filePath)
        {
            // Read the text file as a string
            string data = File.ReadAllText(filePath);

            // Split the string into lines and convert each line back to an object
            List<User> list = new List<User>();
            string[] lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                User obj = (User)Convert.ChangeType(line, typeof(User));
                list.Add(obj);
            }

            return list;
        }
    }
}
