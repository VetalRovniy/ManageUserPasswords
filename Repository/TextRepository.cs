using ManageUserPasswords.Models;
using System;
using System.Collections;
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
                //Clear all data from file (rewrite data)
               
                // Write the string to the text file
                //File.WriteAllText(filePath, data);
                using (StreamWriter writer = new StreamWriter(filePath,false))
                {
                    
                    foreach (User user in list)
                    {
                        string isBlocked = user.IsBlocked?"1":"0";

                        writer.WriteLine($"{user.Username},{user.Password},{isBlocked},{user.RestrictedPassword}");
                    }
                }


                Console.WriteLine("Save succefully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Some error:{ex.Message}");
                return false;
            }
        }

        public List<User> ReadListFromFile(string filePath)
        {
            
            // Read the text file as a string
           
            List<User> list = new List<User>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                   string[] fields = line.Split(',');
                    User usr = new User(fields[0], fields[1]);
                    try
                    {
                        usr.IsBlocked = (fields[2] == "1") ? true : false;
                        usr.RestrictedPassword = fields[3];
                    }
                    catch (Exception ex) {
                        
                    }
                   list.Add(usr);
                }
            }


            return list;
        }
    }
}
