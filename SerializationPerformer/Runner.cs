using SerializationPerformer.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPerformer
{
    class Runner
    {
        private IEnumerable<ISerializer> serializers;

        public Runner(params ISerializer[] serializers)
        {
            this.serializers = serializers;
        }

        public Tuple<double, double, int>[,] Run(IEnumerable<object> data)
        {
            /* Matrix of Tuple<serializeTime, deserializeTime, serializedDataSize> with dataObject on x-axis and serializer (format) on y-axis */
            Tuple<double, double, int>[,] secondsElapsed = new Tuple<double, double, int>[data.Count(), serializers.Count()];
            Stopwatch stopwatch = new Stopwatch();
            int dataCounter = 0;
            int serializerCounter = 0;

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

                    Console.WriteLine($"{serializer.Format}\t : (S) {serializationTime}  \t (DS) {deserializationTime}    \t (B) {serialized.Length}");
                }
                serializerCounter = 0;
                dataCounter++;
                Console.WriteLine("---");
            }

            return secondsElapsed;
        }
    }
}
