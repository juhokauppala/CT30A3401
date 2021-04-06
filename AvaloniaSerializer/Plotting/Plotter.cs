using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaSerializer.Plotting
{
    class Plotter
    {
        public const double BarWidth = 70;

        private static Window window = null;
        private static Grid grid = null;

        private static bool IsInitialized { get => window != null && grid != null; }

        private IBrush[] colors = { Brushes.DarkGoldenrod, Brushes.Azure, Brushes.Crimson, Brushes.SteelBlue, Brushes.OrangeRed };

        public static void Initialize(Window main)
        {
            grid = main.FindControl<Grid>("MainGrid");
            if (grid == null)
                throw new Exception("Plotter initialization errorneous!");
            window = main;
        }

        public Plotter()
        {
            if (!IsInitialized)
                throw new Exception("Using plotter without initializing class!");
        }

        public Plotter(Window window)
        {
            Initialize(window);
        }

        public void Plot(Tuple<double, double, int>[,] data, string[] formats)
        {
            for (int row = 0; row < data.GetLength(0); row++)
            {
                Tuple<double, double, int>[] newRow = new Tuple<double, double, int>[data.GetLength(1)];
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    newRow[col] = data[row, col];
                }

                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

                PlotData(new PlotArgs() { data = newRow, row = row, formats = formats });
            }
        }

        private void PlotData(object dataObj)
        {
            Tuple<double, double, int>[] data = ((PlotArgs)dataObj).data;
            int row = ((PlotArgs)dataObj).row + 1;
            string[] formats = ((PlotArgs)dataObj).formats;

            TextBlock title = new TextBlock() { Text = $"Data{row.ToString()}", FontFamily = "verdana", Foreground = Brushes.Bisque };
            title.SetValue(Grid.ColumnProperty, 0);
            title.SetValue(Grid.RowProperty, row);
            grid.Children.Add(title);

            IControl histogramS = MakeHistogram(data, formats, dataPoint => dataPoint.Item1);
            histogramS.SetValue(Grid.ColumnProperty, 1);
            histogramS.SetValue(Grid.RowProperty, row);
            grid.Children.Add(histogramS);

            IControl histogramD = MakeHistogram(data, formats, dataPoint => dataPoint.Item2);
            histogramD.SetValue(Grid.ColumnProperty, 2);
            histogramD.SetValue(Grid.RowProperty, row);
            grid.Children.Add(histogramD);

            IControl histogram = MakeHistogram(data, formats, dataPoint => dataPoint.Item3);
            histogram.SetValue(Grid.ColumnProperty, 3);
            histogram.SetValue(Grid.RowProperty, row);
            grid.Children.Add(histogram);
        }

        private IControl MakeHistogram(Tuple<double, double, int>[] data, string[] formats, Func<Tuple<double, double, int>, double> dataFromDataPoint)
        {
            HistogramBuilder histogramBuilder = new HistogramBuilder();

            XCounter x = new XCounter(BarWidth, 0, data.Length);
            foreach (Tuple<double, double, int> dataPoint in data)
            {
                int index = data.ToList().IndexOf(dataPoint);
                histogramBuilder.AddBar(x.X, 0, x.X + BarWidth, dataFromDataPoint(dataPoint), formats[index], colors[index]);
                x.NextSerializer();
            }
            return histogramBuilder.Build();
        }

        class PlotArgs
        {
            public Tuple<double, double, int>[] data;
            public int row;
            public string[] formats;
        }
    }
}
