using System;
using Microsoft.Owin.Hosting;
using Nowin;

namespace ken.Spikes.Owin.Nowin
{
    /// <summary>
    /// https://github.com/Bobris/Nowin
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var options = new StartOptions
            {
                ServerFactory = "Nowin",
                Port = 33333
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running at {0}", "http://localhost:" + options.Port);
                Console.ReadKey();
            }
        }
    }
}
