using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class CallException : Exception
    {
        public CallException()
        {
        }

        public CallException(string message)
            : base(message)
        {
        }

        public CallException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}