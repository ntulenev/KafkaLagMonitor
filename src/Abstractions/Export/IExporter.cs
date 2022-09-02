using Models;

namespace Abstractions.Export
{
    public interface IExporter
    {
        public void Export(GroupLagResult groupLag);
    }
}
