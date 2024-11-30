namespace Bookstore.Domain.Entities;

public class Author : IEquatable<Author>
{
    public long Id { get; set; }
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public List<Book> Books { get; set; } = default!;

    public bool Equals(Author? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return other.Firstname == Firstname
            && other.Lastname == Lastname;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(obj as Author);
    }

    public override int GetHashCode()
    {
        return (Firstname, Lastname).GetHashCode();
    }
}
