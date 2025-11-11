using System;

namespace StudentApp.BLL.Infrastructure
{
    public class ValidationException : StudentLogicException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}