namespace Models;

public class GroupLagResult
{
    public GroupLagResult(GroupId group, IEnumerable<PartitionLag> lags)
    {
        Group = group;
        Lags = lags ?? throw new ArgumentNullException(nameof(lags));
    }

    public GroupId Group { get; }

    public IEnumerable<PartitionLag> Lags { get; }
}
