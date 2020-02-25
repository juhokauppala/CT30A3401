using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace SerializationPerformer.Plotting
{
    static class Plotter
    {
        public static void Plot(Tuple<double, double, int>[,] data)
        {
            for (int row = 0; row < data.GetLength(0); row++)
            {
                Tuple<double, double, int>[] newRow = new Tuple<double, double, int>[data.GetLength(1)];
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    newRow[col] = data[row, col];
                }

                new Thread(PlotData).Start(new ThreadStartArgs() { data = newRow, title = $"Data{row + 1}" });
            }
        }

        private static void PlotData(object dataObj)
        {
            Tuple<double, double, int>[] data = ((ThreadStartArgs)dataObj).data;
            string title = ((ThreadStartArgs)dataObj).title;
            Display display = new Display();
            ZedGraphControl control = display.GetZed();
            control.GraphPane = new GraphPane();
            int xIncrement = 0;

            control.GraphPane.Y2Axis.Title.Text = "Time (ms)";
            control.GraphPane.AddY2Axis("Size (b)");

            foreach (Tuple<double, double, int> dataPoint in data)
            {
                PointPairList sPoints = new PointPairList(new double[] { xIncrement, xIncrement + 1 }, new double[] { 0, dataPoint.Item1 });

                PointPairList dPoints = new PointPairList(new double[] { xIncrement + 1, xIncrement + 2 }, new double[] { 0, dataPoint.Item2 });

                PointPairList bPoints = new PointPairList(new double[] { xIncrement + 2, xIncrement + 3 }, new double[] { 0, dataPoint.Item3 });

                control.GraphPane.AddBar("Serialization time", sPoints, Color.Lavender);
                control.GraphPane.AddBar("Deserialization time", dPoints, Color.PowderBlue);
                control.GraphPane.AddBar("Serialized size", bPoints, Color.Honeydew);

                xIncrement += 4;
            }

            control.TopLevelControl.Text = title;



            

            Application.Run(display);
        }

        class ThreadStartArgs
        {
            public Tuple<double, double, int>[] data;
            public string title;
        }
    }
}
