using System;
using Nancy.Hosting.Self;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Proto.Service
{
    public class program
    {
        static void Main()
        {
            SpinUpNancy();
            zeromqserver.SpinUpZeroMQ();

            Console.ReadKey();
            Console.WriteLine("Stopped. Good bye!");
        }
        static async void SpinUpNancy()
        {
            var config = new HostConfiguration();
            //config.RewriteLocalhost = false;
            config.UrlReservations.CreateAutomatically = true;
            NancyHost nh =  await Task.Run(() => RunNancy(config));
            Console.WriteLine("Nancy now listening - navigating to http://localhost:8888/. Press enter to stop");

        }
        static NancyHost RunNancy(HostConfiguration cfg)
        {
            var nancyHost = new NancyHost(cfg,new Uri("http://localhost:8888/"));
            nancyHost.Start();
            return nancyHost;
        }
    }
}