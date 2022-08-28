using Abstractions.Export;

using Models;

using ConsoleTables;

namespace Export
{
    public class ConsoleTableExporter : IExporter
    {
        public void Export(IEnumerable<PartitionLag> data)
        {
            ArgumentNullException.ThrowIfNull(nameof(data));

            var table = new ConsoleTable("Topic", "Partition", "Lag");
            foreach (var item in data)
            {
                table.AddRow(item.Topic, item.PartitionId, item.Lag);
            }
            table.Write();
        }
    }
}
