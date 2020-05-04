using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library.OneTimePassword
{
    public interface IOneTimePasswordGenerator
    {
        OneTimePasswordCode GenerateOneTimePassword(int length = 8);
    }
}
