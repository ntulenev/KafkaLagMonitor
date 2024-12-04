namespace Models;

/// <summary>
/// Represents a group identifier as an immutable, value-based record structure.
/// </summary>
public readonly record struct GroupId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupId"/> struct.
    /// </summary>
    /// <param name="value">The string value of the group identifier.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is null, empty, or contains only whitespace.
    /// </exception>
    public GroupId(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Group id value contains empty string of whitespaces.", nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Gets the string value of the group identifier.
    /// </summary>
    public string Value { get; }
}
