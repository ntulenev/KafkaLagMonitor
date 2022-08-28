namespace Models
{
    public record struct PartitionLag(string Topic, int PartitionId, long Lag);
}
