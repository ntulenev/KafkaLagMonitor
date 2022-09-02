using Abstractions.Logic;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class LagLoader : ILagLoader
    {
        public LagLoader(IOffsetsLagsLoader offsetLoader,
                         ITopicPartitionLoader topicLoader
                        )
        {
            _offsetLoader = offsetLoader ?? throw new ArgumentNullException(nameof(offsetLoader));
            _topicLoader = topicLoader ?? throw new ArgumentNullException(nameof(topicLoader));
        }
        public GroupLagResult LoadOffsetsLags(GroupId groupId, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(groupId);

            var partitions = _topicLoader.LoadPartitions(timeout);
            return _offsetLoader.LoadOffsetsLags(partitions, groupId, timeout);
        }

        private readonly IOffsetsLagsLoader _offsetLoader;
        private readonly ITopicPartitionLoader _topicLoader;
    }
}
