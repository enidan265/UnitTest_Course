using System.Runtime.Serialization;

namespace Bookstore.Application.Exceptions;

[Serializable]
public class IsbnDuplicateException : Exception
{
    public IsbnDuplicateException()
    {
    }

    public IsbnDuplicateException(string? message) : base(message)
    {
    }

    public IsbnDuplicateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected IsbnDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
