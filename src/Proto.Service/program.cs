using System;
using Nancy.Hosting.Self;
using System.Diagnostics;

namespace Proto.Service
{
    public class program
    {
        static void Main()
        {
            var config = new HostConfiguration();
            //config.RewriteLocalhost = false;
            config.UrlReservations.CreateAutomatically = true;
            var nancyHost = new NancyHost(config,new Uri("http://localhost:8888/"));
            nancyHost.Start();

            Console.WriteLine("Nancy now listening - navigating to http://localhost:8888/. Press enter to stop");
            Console.ReadKey();

            nancyHost.Stop();

            Console.WriteLine("Stopped. Good bye!");
        }
    }
}