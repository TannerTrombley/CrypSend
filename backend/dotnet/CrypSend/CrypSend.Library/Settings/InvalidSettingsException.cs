using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.Settings
{
    public class InvalidSettingsException : Exception
    {
        public InvalidSettingsException(string message) : base(message)
        { }
    }
}
