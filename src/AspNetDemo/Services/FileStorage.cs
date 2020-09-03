using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetDemo.Services
{
    public static class FileStorage
    {
        public static string GetEmailProofIdFromFile()
        {
            return GetFromFile("emailProofId.txt");
        }

        public static void StoreEmailProofIdFromFile(string emailProofId)
        {
            WriteToFile("emailProofId.txt", emailProofId);
        }

        public static string GetEmailCredDefIdFromFile()
        {
            return GetFromFile("emailCredDefId.txt");
        }

        public static void StoreEmailCredDefIdToFile(string emailCredDefId)
        {
            WriteToFile("emailCredDefId.txt", emailCredDefId);
        }

        public static string GetRoleCredDefIdFromFile()
        {
            return GetFromFile("roleCredDefId.txt");
        }

        public static void StoreRoleCredDefIdToFile(string emailCredDefId)
        {
            WriteToFile("roleCredDefId.txt", emailCredDefId);
        }

        private static string GetFromFile(string fileName)
        {
            string content = null;
            try
            {
                content = System.IO.File.ReadAllText($"ids/{fileName}");
            }
            catch (Exception) { }
            return content;
        }

        private static void WriteToFile(string fileName, string content)
        {
            System.IO.File.WriteAllText(fileName, content);
        }
    }
}
