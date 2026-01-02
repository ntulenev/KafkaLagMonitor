using Abstractions.Export;

using ConsoleTables;

using Models;

namespace Export;

/// <summary>
/// A class that exports the GroupLagResult data to the console in tabular format.
/// </summary>
public class ConsoleTableExporter : IExporter
{

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
    public void Export(GroupLagResult groupLag)
    {
        ArgumentNullException.ThrowIfNull(groupLag);

        BuildTitle(groupLag);
        FillTable(groupLag);
    }

    private static void BuildTitle(GroupLagResult data)
    {
#pragma warning disable IDE0022 // Use expression body for method
        Console.WriteLine($"Group - {data.Group.Value}");
#pragma warning restore IDE0022 // Use expression body for method
    }

    private static void FillTable(GroupLagResult data)
    {
        var table = new ConsoleTable("Topic", "Partition", "Lag");

        foreach (var item in data.Lags)
        {
            _ = table.AddRow(item.Topic, item.PartitionId, item.Lag);
        }

        table.Write();
    }
}
