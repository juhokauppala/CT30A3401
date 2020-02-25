using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPerformer.Serialization
{
    class JSON : ISerializer
    {
        private JsonSerializer serializer = new JsonSerializer();
        public string Format => "JSON        ";

        public object Deserialize(byte[] data)
        {
            //return serializer.Deserialize(new StringReader(data), typeof(string));
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data));
        }

        public byte[] Serialize(object data)
        {
            /*
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, data);
            return writer.ToString();
            */
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}
