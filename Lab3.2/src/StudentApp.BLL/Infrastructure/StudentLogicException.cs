using System;

namespace StudentApp.BLL.Infrastructure
{
    public class StudentLogicException : Exception
    {
        public StudentLogicException(string message) : base(message) { }
    }
}