using System;

namespace Insurance.Core.Exceptions
{
    public class KnownException : Exception
    {
        public int StatusCode { get; }

        public KnownException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}