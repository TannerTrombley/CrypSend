using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.OneTimePassword
{
    public class OneTimePasswordCode
    {
        public OneTimePasswordCode(string code)
        {
            Code = code;

        }

        public string Code
        {
            get;
            private set;
        }

        public BitArray CodeBits
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(Code);
                return new BitArray(bytes);
            }
        }
    }
}
