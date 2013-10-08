using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Xemio.SmartNotes.AuthenticationHashTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Content: ");
            string content = Console.ReadLine();

            byte[] authenticationBytes = Encoding.UTF8.GetBytes(username + password);
            byte[] authenticationHash = SHA256.Create().ComputeHash(authenticationBytes);

            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] contentHash = new HMACSHA256(authenticationHash).ComputeHash(contentBytes);
            string contentHashString = Convert.ToBase64String(contentHash);

            Console.WriteLine("Username: {0}", username);
            Console.WriteLine("Password: {0}", password);
            Console.WriteLine("Content: {0}", content);

            Console.WriteLine("-----------------------------------------");

            Console.WriteLine("AuthenticationBytes: {0}", BitConverter.ToString(authenticationBytes));
            Console.WriteLine("AuthenticationHash: {0}", BitConverter.ToString(authenticationHash));
            Console.WriteLine("AuthenticationHash (string): {0}", Convert.ToBase64String(authenticationHash));

            Console.WriteLine("ContentBytes: {0}", BitConverter.ToString(contentBytes));
            Console.WriteLine("ContentHash: {0}", BitConverter.ToString(contentHash));
            Console.WriteLine("ContentHashString: {0}", contentHashString);

            Console.ReadLine();
        }
    }
}
