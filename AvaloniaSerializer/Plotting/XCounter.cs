using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaSerializer.Plotting
{
    class XCounter
    {
        private double barWidth;
        private int numSerializers;
        private int currentSerializer;

        public double X { get; private set; }


        public XCounter(double barWidth, double initX, int numSerializers)
        {
            this.barWidth = barWidth;
            this.numSerializers = numSerializers;
            X = initX;
            currentSerializer = 0;
        }

        public void NextSerializer()
        {
            if (++currentSerializer < numSerializers)
                X += barWidth;
            else
            {
                X += barWidth * 2;
                currentSerializer = 0;
            }
        }
    }
}
