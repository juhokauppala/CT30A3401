using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPerformer.Serialization
{
    class MP : ISerializer
    {
        private Type dataType;
        public string Format => "MessagePack";

        public object Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize(dataType, data);
        }

        public byte[] Serialize(object data)
        {
            dataType = data.GetType();
            return MessagePackSerializer.Serialize(dataType, data);
        }
    }
}
