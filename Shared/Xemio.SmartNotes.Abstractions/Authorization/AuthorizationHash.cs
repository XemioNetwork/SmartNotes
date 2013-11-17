﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Abstractions.Authorization
{
    /// <summary>
    /// Contains the whole logic creating a authorization hash.
    /// </summary>
    public static class AuthorizationHash
    {
        /// <summary>
        /// Creates the authorization hash from the given <paramref name="username"/>, <paramref name="password"/> and <paramref name="content"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="content">The content.</param>
        public static string Create(string username, string password, string content = "")
        {
            byte[] baseHash = CreateBaseHash(username, password);

            return Create(baseHash, content);
        }
        /// <summary>
        /// Creates the authorization hash from the given <paramref name="baseHash"/> and <paramref name="content"/>.
        /// </summary>
        /// <param name="baseHash">The authorization hash.</param>
        /// <param name="content">The content.</param>
        public static string Create(byte[] baseHash, string content = "")
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] contentHash = new HMACSHA256(baseHash).ComputeHash(contentBytes);
            return Convert.ToBase64String(contentHash);
        }
        /// <summary>
        /// Creates the base hash from the given <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static byte[] CreateBaseHash(string username, string password)
        {
            byte[] authorizationBytes = Encoding.UTF8.GetBytes(username + password);
            return SHA256.Create().ComputeHash(authorizationBytes);
        }
    }
}