using CrypSend.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public enum RetrievalConditionType
    {
        None,
        OTP
    }

    public class RetrievalCondition
    {
        public bool HasMetCondition { get; set; }

        public RetrievalConditionType Type { get; set; }
    }
}
