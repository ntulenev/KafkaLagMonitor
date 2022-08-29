using Models;

namespace Abstractions.Logic
{
    public interface ILagLoader
    {
        public IReadOnlyCollection<PartitionLag> LoadOffsetsLags(GroupId groupId, TimeSpan timeout);
    }
}
