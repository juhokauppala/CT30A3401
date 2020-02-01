using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Logging
{
    public class Logger
    {
        private int MinTop;
        private int NextInfoLine;
        private readonly object WriteLock = new object();

        private static Logger instance = null;
        private Logger()
        {
            MinTop = Console.CursorTop;
            NextInfoLine = MinTop + 2;
            Console.WriteLine("Connected clients    Time of loop    Messages received");
            Console.CursorVisible = false;
        }

        public static Logger GetInstance()
        {
            if (instance == null)
                instance = new Logger();

            return instance;
        }

        private string GetStaticInfoString(int numClients, TimeSpan time, int numMessages)
        {
            return string.Format("{0,17}    {1,12}    {2,17}", numClients, time, numMessages);
        }

        public void UpdateStaticLine(int numClients, TimeSpan time, int numMessages)
        {
            lock (WriteLock)
            {
                int left = Console.CursorLeft;
                for (int i = left; i >= 0; i--)
                {
                    Console.SetCursorPosition(i, MinTop + 1);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(0, MinTop + 1);
                Console.Write(GetStaticInfoString(numClients, time, numMessages));
                Console.SetCursorPosition(0, NextInfoLine);
            }   
        }

        public void NewInfoLine(string message)
        {
            lock (WriteLock)
            {
                Console.SetCursorPosition(0, NextInfoLine);
                Console.WriteLine(message);
                NextInfoLine++;
            }
        }
    }
}
