using System;
using SignalR.Owin.Relay.Client;

namespace RelayClientTest
{
    class Program
    {

        static void Main(string[] args)
        {
            var relayClient = RelayClient.Create<NancyStartup>("http://localhost:42705/relay");
            using (relayClient.Start().Result)
            {
                Console.ReadKey(true);
            }
        }
    }

}