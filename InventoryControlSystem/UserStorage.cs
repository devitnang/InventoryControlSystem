using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InventoryControlSystem
{
    public class UserStorage
    {
        private static string filePath = "lastuser.txt";

        public static void SaveLastUser(string username)
        {
            File.WriteAllText(filePath, username);
        }

        public static string LoadLastUsername()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return string.Empty;
        }
    }
}
