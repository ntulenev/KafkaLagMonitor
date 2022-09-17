using Abstractions.Export;

using Models;

using ConsoleTables;

namespace Export
{
    /// <summary>
    /// Simple console result exporter.
    /// </summary>
    public class ConsoleTableExporter : IExporter
    {
        /// <summary>
        /// Exports <see cref="GroupLagResult"/> to Cosnole.
        /// </summary>
        /// <param name="data">Reulst data.</param>
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
}
