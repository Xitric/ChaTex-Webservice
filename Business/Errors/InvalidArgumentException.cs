using System;

namespace Business.Errors
{
    public class InvalidArgumentException : ArgumentException
    {
        public new ParamNameType ParamName;
        public InvalidArgumentException(string message, ParamNameType paramNameType) {
            ParamName = paramNameType;
        }
    }
}
