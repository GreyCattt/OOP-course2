namespace StoreApp.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
    }

    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
    }
}