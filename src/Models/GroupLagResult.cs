namespace Models;

/// <summary>
/// Represents the lag results of a group, including the group identifier and the partition lags.
/// </summary>
public class GroupLagResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupLagResult"/> class.
    /// </summary>
    /// <param name="group">The group identifier associated with the lag results.</param>
    /// <param name="lags">The collection of partition lags for the group.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="lags"/> is null.</exception>
    public GroupLagResult(GroupId group, IEnumerable<PartitionLag> lags)
    {
        Group = group;
        Lags = lags ?? throw new ArgumentNullException(nameof(lags));
    }

    /// <summary>
    /// Gets the group identifier associated with the lag results.
    /// </summary>
    public GroupId Group { get; }

    /// <summary>
    /// Gets the collection of partition lags for the group.
    /// </summary>
    public IEnumerable<PartitionLag> Lags { get; }
}
