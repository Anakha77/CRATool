using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;
using log4net.Config;

namespace CRATool.TestServer
{
    class Program
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            var loggerConfigFileInfo = new FileInfo("log4net.config.xml");
            XmlConfigurator.ConfigureAndWatch(loggerConfigFileInfo);

            RunServer();

            Console.ReadLine();
        }

        private static void RunServer()
        {
            var server = new TcpListener(IPAddress.Any, 7391);
            var service = new Service.ToDoRename();

            try
            {
                server.Start();

                while (true)
                {
                    service.Listening(server, Log);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }
            finally
            {
                server.Stop();
                Log.Warn("SERVER STOP");
            }
        }
    }
}
