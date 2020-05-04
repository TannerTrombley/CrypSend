using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.OneTimePassword
{
    public class OneTimePasswordGenerator : IOneTimePasswordGenerator
    {
        private const string AVAILABLE_CHARS = "0123456789";

        public OneTimePasswordCode GenerateOneTimePassword(int length = 8)
        {
            var rand = new Random();
            var code = "";
            for (int i = 0; i < length; i++)
            {
                code += AVAILABLE_CHARS[rand.Next(0, AVAILABLE_CHARS.Length - 1)];
            }

            return new OneTimePasswordCode(code);
        }
    }
}
