﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Authorization
{
    /// <summary>
    /// Contains the whole logic creating a authorization hash.
    /// </summary>
    public static class AuthorizationHash
    {
        /// <summary>
        /// The number of iterations for the PBKDF2 algorithm.
        /// </summary>
        public const int Iterations = 2000;

        /// <summary>
        /// Creates the authorization hash from the given <paramref name="username"/>, <paramref name="password"/> and <paramref name="content"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="requestDate">The request date.</param>
        /// <param name="content">The content.</param>
        public static async Task<string> Create(string username, string password, DateTimeOffset requestDate, string content = "")
        {
            byte[] baseHash = await CreateBaseHash(username, password);

            return Create(baseHash, requestDate, content);
        }
        /// <summary>
        /// Creates the authorization hash from the given <paramref name="baseHash"/> and <paramref name="content"/>.
        /// </summary>
        /// <param name="baseHash">The authorization hash.</param>
        /// <param name="requestDate">The request date.</param>
        /// <param name="content">The content.</param>
        public static string Create(byte[] baseHash, DateTimeOffset requestDate, string content = "")
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content + requestDate.UtcDateTime.ToString("yyyy-MM-ddThh:mm:ssZ"));
            byte[] contentHash = new HMACSHA256(baseHash).ComputeHash(contentBytes);
            return Convert.ToBase64String(contentHash);
        }
        /// <summary>
        /// Creates the base hash from the given <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static Task<byte[]> CreateBaseHash(string username, string password)
        {
            return Task.Run(() =>
            {
                if (username == null)
                    username = string.Empty;

                if (password == null)
                    password = string.Empty;

                byte[] authorizationBytes = Encoding.UTF8.GetBytes(username + password);
                return SHA256.Create().ComputeHash(authorizationBytes);
            });
        }
    }
}