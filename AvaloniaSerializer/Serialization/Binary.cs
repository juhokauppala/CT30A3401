using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSerializer.Serialization
{
    class Binary : ISerializer
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        public string Format => "Binary";

        public object Deserialize(byte[] data)
        {
            return formatter.Deserialize(new MemoryStream(data));
        }

        public byte[] Serialize(object data)
        {
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, data);
            return stream.ToArray();
        }
    }
}
