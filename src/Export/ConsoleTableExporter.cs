using Abstractions.Export;

using Models;

using ConsoleTables;

namespace Export
{
    internal class ConsoleTableExporter : IExporter
    {
        public void Export(IEnumerable<PartitionLag> data, string groupId)
        {
            ArgumentNullException.ThrowIfNull(nameof(data));
            ArgumentNullException.ThrowIfNull(nameof(groupId));

            Console.WriteLine($"GroupId - {groupId}");
            var table = new ConsoleTable("Topic", "Partition", "Lag");
            foreach (var item in data)
            {
                table.AddRow(item.Topic, item.PartitionId, item.Lag);
            }
            table.Write();
        }
    }
}
