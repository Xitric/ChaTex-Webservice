using System;

namespace Business.Errors
{
    /// <summary>
    /// Modification of the ArgumentException to support enum parameter types rather than parameter names as strings. This is to ensure that typos do not occur.
    /// </summary>
    public class InvalidArgumentException : ArgumentException
    {
        public new ParamNameType ParamName;
        public InvalidArgumentException(string message, ParamNameType paramNameType) : base(message)
        {
            ParamName = paramNameType;
        }
    }
}
