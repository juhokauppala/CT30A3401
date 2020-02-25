using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPerformer.Serialization
{
    interface ISerializer
    {
        string Format { get; }

        byte[] Serialize(object data);
        object Deserialize(byte[] data);
    }
}
