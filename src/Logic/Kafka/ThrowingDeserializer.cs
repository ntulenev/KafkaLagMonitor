using Confluent.Kafka;

namespace Logic.Kafka;

/// <summary>
/// Stub Kafka Deserializer.
/// </summary>
public class ThrowingDeserializer : IDeserializer<byte[]>
{
    private static readonly Lazy<ThrowingDeserializer> _lazy = new(() => new ThrowingDeserializer());

    /// <summary>
    /// Singleton instance of <see cref="ThrowingDeserializer"/>.
    /// </summary>
    public static ThrowingDeserializer Instance => _lazy.Value;

    private ThrowingDeserializer() { }

    /// <summary>
    /// Deserialize any byte data with throwing NotSupportedException.
    /// </summary>
    /// <exception cref="NotSupportedException">Throws for any data.</exception>
    public byte[] Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        => throw new NotSupportedException();
}
