using Microsoft.Owin.Hosting;

namespace ken.Spikes.Owin.Console
{
    /// <summary>
    /// Using HttpListener
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            const string url = "http://localhost:11111";
            using (WebApp.Start<Startup>(url))
            {
                System.Console.WriteLine("Running at {0}", url);
                System.Console.ReadLine();
            }
        }
    }
}
