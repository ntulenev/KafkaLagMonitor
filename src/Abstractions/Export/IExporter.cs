using Models;

namespace Abstractions.Export;

/// <summary>
/// Lag result exporter.
/// </summary>
public interface IExporter
{
    /// <summary>
    /// Exports lag results.
    /// </summary>
    /// <param name="groupLag">Lag result for group.</param>
    void Export(GroupLagResult groupLag);
}

