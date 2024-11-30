namespace Bookstore.Domain.Entities;

public class Book : IEquatable<Book>
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string Isbn { get; set; } = default!;
    public int Quantity { get; set; }
    public long AuthorId { get; set; }
    public Author Author { get; set; } = default!;

    public bool Equals(Book? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return other.Isbn == Isbn &&
            other.AuthorId == AuthorId &&
            other.Quantity == Quantity &&
            other.Title == Title;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals(obj as Book);
    }

    public override int GetHashCode()
    {
        return (Title, Isbn, AuthorId, Quantity).GetHashCode();
    }
}
