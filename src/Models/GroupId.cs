namespace Models;

public readonly record struct GroupId
{
    public GroupId(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Group id value contains empty string of whitespaces.", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }
}
