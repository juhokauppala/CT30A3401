using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvaloniaSerializer.Serialization
{
    class XML : ISerializer
    {
        XmlSerializer serializer;
        public string Format => "XML";

        public object Deserialize(byte[] data)
        {
            StringReader reader = new StringReader(Encoding.UTF8.GetString(data));
            return serializer.Deserialize(reader);
        }

        public byte[] Serialize(object data)
        {
            StringWriter writer = new StringWriter();
            Type t = data.GetType();
            serializer = new XmlSerializer(t);
            serializer.Serialize(writer, data);
            return Encoding.UTF8.GetBytes(writer.ToString());
        }
    }
}
