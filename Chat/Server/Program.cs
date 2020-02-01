using Server.Logging;
using System;

namespace Server
{
    class Program
    {
        private static Server.Server server = null;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += CtrlCHandler;

            if (!(args.Length > 0 && int.TryParse(args[0], out int port)))
            {
                port = 3401;
                Logger.GetInstance().NewInfoLine($"Using default port: {port}");
            }
            else
            {
                Logger.GetInstance().NewInfoLine($"Using custom port: {port}");
            }

            server = new Server.Server();
            server.Start(port);
        }

        protected static void CtrlCHandler(object sender, ConsoleCancelEventArgs args)
        {
            Logger.GetInstance().NewInfoLine("Stopping server gracefully...");
            if (server != null)
                server.Stop();
        }
    }
}
