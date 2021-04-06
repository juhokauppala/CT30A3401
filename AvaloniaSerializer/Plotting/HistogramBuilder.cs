using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaSerializer.Plotting
{
    class HistogramBuilder
    {
        public const double bottomSpace = 30;
        private Canvas canvas;

        public HistogramBuilder()
        {
            canvas = new Canvas();
            canvas.Height = 200;
            canvas.Width = 400;
            canvas.Background = Brushes.Bisque;
        }

        public HistogramBuilder AddBar(double x0, double y0, double x1, double y1, string label, IBrush color)
        {
            Panel bar = new Panel();
            bar.SetValue(Canvas.BottomProperty, Math.Min(y0, y1) + bottomSpace);
            bar.SetValue(Canvas.LeftProperty, Math.Min(x0, x1));
            bar.SetValue(Canvas.BackgroundProperty, color);
            bar.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            bar.SetValue(Border.BorderBrushProperty, Brushes.Black);
            bar.Width = Math.Abs(x0 - x1);
            bar.Height = Math.Abs(y0 - y1);
            canvas.Children.Add(bar);
            TextBlock labelText = new TextBlock();
            labelText.SetValue(Canvas.BottomProperty, Math.Min(y0, y1));
            labelText.SetValue(Canvas.LeftProperty, Math.Min(x0, x1));
            labelText.TextAlignment = TextAlignment.Center;
            labelText.Width = Math.Abs(x1 - x0);
            labelText.Text = label;
            canvas.Children.Add(labelText);
            TextBlock value = new TextBlock();
            value.Text = Convert.ToString(Math.Round(Math.Abs(y1 - y0), 2));
            value.SetValue(Canvas.BottomProperty, Math.Min(y0, y1) + bottomSpace);
            value.SetValue(Canvas.LeftProperty, Math.Min(x0, x1));
            value.Width = Math.Abs(x1 - x0);
            value.TextAlignment = TextAlignment.Center;
            canvas.Children.Add(value);
            return this;
        }

        public IControl Build()
        {
            double scaler = (canvas.Height - bottomSpace*2) / canvas.Children.Where(c => c.GetType() == typeof(Panel)).Select(p => p.Height).Max();
            foreach (Panel bar in canvas.Children.Where(c => c.GetType() == typeof(Panel)))
            {
                bar.Height *= scaler;
            }
            return canvas;
        }
    }
}
