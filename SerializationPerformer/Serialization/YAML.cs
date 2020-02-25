using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SerializationPerformer.Serialization
{
    class YAML : ISerializer
    {
        public string Format => "YAML        ";

        public object Deserialize(byte[] data)
        {
            return new Deserializer().Deserialize(new StringReader(Encoding.UTF8.GetString(data)));
        }

        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(new Serializer().Serialize(data));
        }
    }
}
