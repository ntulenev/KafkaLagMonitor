using Models;

namespace Abstractions.Logic;

/// <summary>
/// Kafka lag loader.
/// </summary>
public interface ILagLoader
{
    /// <summary>
    /// Loads lags for group.
    /// </summary>
    /// <param name="groupId">GroupId.</param>
    /// <param name="timeout">Load timeout.</param>
    /// <returns>Lags for group.</returns>
    GroupLagResult LoadOffsetsLags(GroupId groupId, TimeSpan timeout);
}
