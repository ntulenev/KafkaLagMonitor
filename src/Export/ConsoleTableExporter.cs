using Abstractions.Export;

using Models;

using ConsoleTables;

namespace Export
{
    public class ConsoleTableExporter : IExporter
    {
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
