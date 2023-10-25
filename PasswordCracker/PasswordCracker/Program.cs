using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace PasswordCracker
{
    /// <summary>
    /// A list of md5 hashed passwords is contained within the passwords_hashed.txt file.  Your task
    /// is to crack each of the passwords.  Your input will be an array of strings obtained by reading
    /// in each line of the text file and your output will be validated by passing an array of the
    /// cracked passwords to the Validator.ValidateResults() method.  This method will compute a SHA256
    /// hash of each of your solved passwords and compare it against a list of known hashes for each
    /// password.  If they match, it means that you correctly cracked the password.  Be warned that the
    /// test is ALL or NOTHING.. so one wrong password means the test fails.
    /// </summary>
    class Program
    {
        public static string md5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        static void Main(string[] args)
        {
            string[] hashedPasswords = File.ReadAllLines("passwords_hashed.txt");
            Dictionary<string, string> passwords = new Dictionary<string, string>();
            Dictionary<string, string> randomPass = new Dictionary<string, string>();
            string[] commonWords = File.ReadAllLines("most_common.txt");
            string[] crackedPasswords = new string[hashedPasswords.Length];

            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    for (int k = 0; k < alphabet.Length; k++)
                    {
                        for (int l = 0; l < alphabet.Length; l++)
                        {
                            for (int m = 0; m < alphabet.Length; m++)
                            {
                                string password = alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString() + alphabet[l].ToString() + alphabet[m].ToString();
                                string hash = md5(password);
                                randomPass.Add(hash, password);
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine("MD5 Password Cracker v1.0");

            // "ABCD12342332233232" -> "money"

            // ideas:
            // 1. randomly select 5 letters, make them a string, convert to hash, compare (takes way too long, all the passwords are likely actual words)
            // 2. find most common 5 letter words, compare hashes, if none work then try random 5 letter strings as backup (maybe)
            // TODO: make #2

            foreach (var word in commonWords)
            {
                var hash = md5(word);

                passwords.Add(hash, word);

            }

            int a = 0;
            foreach (var hash in hashedPasswords)
            {
                if (passwords.ContainsKey(hash)!)
                {
                    string pass = passwords[hash];
                    Console.WriteLine($"{hash} - {pass}");
                    crackedPasswords[a] = pass;
                    a++;
                }
                else if (randomPass.ContainsKey(hash)!)
                {
                    string pass = randomPass[hash];
                    Console.WriteLine($"{hash} - {pass}");
                    crackedPasswords[a] = pass;
                    a++;
                }
            }

            // Use this method to test if you managed to correctly crack all the passwords
            // Note that hashedPasswords will need to be swapped out with an array the exact
            // same length that contains all the cracked passwords
            bool passwordsValidated = Validator.ValidateResults(crackedPasswords);

            Console.WriteLine($"\nPasswords successfully cracked: {passwordsValidated}");
        }
}
}