using Confluent.Kafka;

namespace Logic.Kafka
{
    public class ThrowingDeserializer : IDeserializer<byte[]>
    {
        private static readonly Lazy<ThrowingDeserializer> lazy = new(() => new ThrowingDeserializer());

        public static ThrowingDeserializer Instance => lazy.Value;

        private ThrowingDeserializer() { }

        public byte[] Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            throw new NotSupportedException();
        }
    }
}
