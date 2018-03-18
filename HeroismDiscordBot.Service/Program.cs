using System;
using System.ServiceProcess;

namespace HeroismDiscordBot.Service
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
#if DEBUG
            var service = new Service();
            service.StartService();

            Console.ReadKey();
            service.Stop();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                            {
                                new Service()
                            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}