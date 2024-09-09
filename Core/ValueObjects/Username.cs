namespace Core.ValueObjects;

public class Username
{
    public string Value { get; }

    // Constructor enforces the validation rules
    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(value));
        }

        Value = value;
    }

    // Override equality operators (important for ValueObjects)
    public override bool Equals(object? obj)
    {
        if (obj is Username other)
        {
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public static implicit operator string(Username username) => username.Value;
    
    public override string ToString() => Value;
}