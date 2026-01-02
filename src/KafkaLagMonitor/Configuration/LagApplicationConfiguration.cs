using Models;

namespace KafkaLagMonitor.Configuration;

/// <summary>
/// Lag application configuration.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public class LagApplicationConfiguration
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// List of groups where we need to check the offsets.
    /// </summary>
#pragma warning disable CA1721 // Property names should not match get methods
#pragma warning disable CA1002 // Do not expose generic lists
    public List<string> Groups { get; init; } = null!;
#pragma warning restore CA1002 // Do not expose generic lists
#pragma warning restore CA1721 // Property names should not match get methods

    /// <summary>
    /// Offsets request timeout.
    /// </summary>
    public TimeSpan Timeout { get; init; }

    /// <summary>
    /// Gets Group Ids.
    /// </summary>
    public IReadOnlyCollection<GroupId> GetGroups() => [.. Groups.Select(x => new GroupId(x))];
}
