using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaSerializer.Plotting;
using AvaloniaSerializer.Serialization;
using System;

namespace AvaloniaSerializer
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Run();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Run()
        {
            Runner runner = new Runner(new Binary(), new JSON(), new XML(), new MP(), new YAML());
            Tuple<Tuple<double, double, int>[,], string[]> data = runner.Run(Data.Data.DataObjects);
            Plotter plotter = new Plotter(this);
            plotter.Plot(data.Item1, data.Item2);
        }
    }
}
