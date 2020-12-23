using System;
using System.IO;


namespace AspNetDemo.Services
{
    /// <summary>
    /// Some helpers for caching and retrieving some IDs that we'd rather not generate fresh every time. This utility simply
    /// stores IDs in files in the <code>/ids</code> directory.
    /// </summary>
    public static class FileStorage
    {
        public static string GetEmailProofIdFromFile()
        {
            return GetFromFile("emailProofId.txt");
        }

        public static void StoreEmailProofIdToFile(string emailProofId)
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
                content = File.ReadAllText($"ids/{fileName}");
            }
            catch (Exception) { }
            return content;
        }

        private static void WriteToFile(string fileName, string content)
        {
            Directory.CreateDirectory("ids");

            File.WriteAllText($"ids/{fileName}", content);
        }
    }
}
