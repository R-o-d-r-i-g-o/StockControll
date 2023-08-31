using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StockControll.Commons
{
    public static class AuthSettings
    {
        public static string CalculateMD5(string input)
        {
            using (MD5 md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes) {
                    builder.Append(b.ToString("x2")); // Convert byte to hex format
                }
                return builder.ToString();
            }
        }
    }
}