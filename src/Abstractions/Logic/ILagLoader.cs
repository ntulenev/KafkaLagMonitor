using Models;

namespace Abstractions.Logic
{
    public interface ILagLoader
    {
        public GroupLagResult LoadOffsetsLags(GroupId groupId, TimeSpan timeout);
    }
}
