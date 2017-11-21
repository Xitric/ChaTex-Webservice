using System;
using System.Collections.Generic;
using System.Text;

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
