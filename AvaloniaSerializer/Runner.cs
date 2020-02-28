using AvaloniaSerializer.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSerializer
{
    class Runner
    {
        private IEnumerable<ISerializer> serializers;

        public Runner(params ISerializer[] serializers)
        {
            this.serializers = serializers;
        }

        public Tuple<Tuple<double, double, int>[,], string[]> Run(IEnumerable<object> data)
        {
            /* Matrix of Tuple<serializeTime, deserializeTime, serializedDataSize> with dataObject on x-axis and serializer (format) on y-axis */
            Tuple<double, double, int>[,] secondsElapsed = new Tuple<double, double, int>[data.Count(), serializers.Count()];
            string[] formats = new string[serializers.Count()];
            Stopwatch stopwatch = new Stopwatch();
            int dataCounter = 0;
            int serializerCounter = 0;

            Debug.WriteLine($"{"FORMAT",15} | {"Serialization / ms",20} {"Deserialization / ms",20} {"Size / B",8}");
            Debug.WriteLine("");

            foreach (object datum in data)
            {
                foreach (ISerializer serializer in serializers)
                {
                    stopwatch.Restart();
                    byte[] serialized = serializer.Serialize(datum);
                    double serializationTime = stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Restart();
                    object deserialized = serializer.Deserialize(serialized);
                    double deserializationTime = stopwatch.Elapsed.TotalMilliseconds;

                    secondsElapsed[dataCounter, serializerCounter] = new Tuple<double, double, int>(serializationTime, deserializationTime, serialized.Length);
                    serializerCounter++;

                    Debug.WriteLine($"{serializer.Format,-15} | {serializationTime,20} {deserializationTime,20} {serialized.Length,8}");

                    if (data.First() == datum)
                        formats[serializers.ToList().IndexOf(serializer)] = serializer.Format;
                }
                serializerCounter = 0;
                dataCounter++;
                Debug.WriteLine("---");
            }

            return new Tuple<Tuple<double, double, int>[,], string[]>(secondsElapsed, formats);
        }
    }
}
