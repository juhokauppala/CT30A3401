using SerializationPerformer.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPerformer
{
    class Program
    {
        static void Main(string[] args)
        {

            Runner runner = new Runner(new Binary(), new JSON(), new XML(), new MP(), new YAML());
            Plotting.Plotter.Plot(runner.Run(Data.Data.DataObjects));

            Console.Write("Press enter to quit... ");
            Console.ReadLine();
        }


    }
}
