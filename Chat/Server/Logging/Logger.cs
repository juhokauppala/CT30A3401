using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Logging
{
    public class Logger
    {
        private int NextInfoLine;
        private int CurrentStaticLine;
        private readonly object WriteLock = new object();

        private static Logger instance = null;
        private Logger()
        {
            NextInfoLine = Console.CursorTop;
            CurrentStaticLine = -1;
            Console.CursorVisible = false;
        }

        public static Logger GetInstance()
        {
            if (instance == null)
                instance = new Logger();

            return instance;
        }

        private string GetStaticInfoString(int numClients, TimeSpan time, int numMessages, bool headersToo = false)
        {
            string result = "";

            if (headersToo)
                result += "Connected clients    Time of loop    Messages received\n";

            result = result + "{0,17}    {1,12}    {2,17}";
            return string.Format(result, numClients, time, numMessages);
        }

        public void UpdateStaticLine(int numClients, TimeSpan time, int numMessages)
        {
            lock (WriteLock)
            {
                EraseStatic(false);

                bool headersToo = CurrentStaticLine < 0;
                string line = GetStaticInfoString(numClients, time, numMessages, headersToo);

                CurrentStaticLine = NextInfoLine + 1;
                int writeLine = CurrentStaticLine + (headersToo ? 0 : 1);
                Console.SetCursorPosition(0, writeLine);
                Console.Write(line);
            }   
        }

        public void NewInfoLine(string message)
        {
            lock (WriteLock)
            {
                EraseStatic();
                CurrentStaticLine = -1;
                Console.SetCursorPosition(0, NextInfoLine);
                Console.WriteLine(message);
                NextInfoLine += message.Split("\n").Length;
            }
        }

        private void EraseStatic(bool infoToo = true)
        {
            if (CurrentStaticLine >= 0)
            {
                if (infoToo)
                    EraseLine(CurrentStaticLine);
                EraseLine(CurrentStaticLine + 1);
            }
        }

        private void EraseLine(int lineNumber)
        {
            if (lineNumber < 0)
                return;

            int originalLeft = Console.CursorLeft;
            int originalTop = Console.CursorTop;

            Console.CursorTop = lineNumber;
            int left = Console.BufferWidth - 1;
            for (int i = left; i >= 0; i--)
            {
                Console.SetCursorPosition(i, lineNumber);
                Console.Write(" ");
            }

            Console.SetCursorPosition(originalLeft, originalTop);
        }
    }
}
