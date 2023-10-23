using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordCracker
{
    public static class Validator
    {
        public static bool ValidateResults(string[] lines)
        {
            string[] hashed = lines.Select(n => ComputeSha256Hash(n)).ToArray();
            string[] pwdata = File.ReadAllLines("passwords_validate.txt");

            return hashed.SequenceEqual(pwdata);
        }
        
        private static string ComputeSha256Hash(string rawData)  
        {  
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        }          
    }
}