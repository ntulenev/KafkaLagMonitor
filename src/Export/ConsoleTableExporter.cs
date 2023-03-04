using Abstractions.Export;

using Models;

using ConsoleTables;

namespace Export;

/// <summary>
/// A class that exports the GroupLagResult data to the console in tabular format.
/// </summary>
public class ConsoleTableExporter : IExporter
{

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
    public void Export(GroupLagResult data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Console.WriteLine($"Group - {data.Group.Value}");

        var table = new ConsoleTable("Topic", "Partition", "Lag");

        foreach (var item in data.Lags)
        {
            table.AddRow(item.Topic, item.PartitionId, item.Lag);
        }

        table.Write();
    }
}
