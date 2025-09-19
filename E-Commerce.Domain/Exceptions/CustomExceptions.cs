namespace E_Commerce.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(List<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}