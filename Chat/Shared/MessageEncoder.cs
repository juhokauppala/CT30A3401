using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    public static class MessageEncoder
    {
        public static Message Decode(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            return (Message)formatter.Deserialize(stream);
        }

        public static byte[] Encode(Message message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return stream.ToArray();
        }
    }
}
