using System;
using Nancy;
using SignalR.Owin.Relay.Common;

namespace RelayClientTest
{
    public class TestClientRelayModule : NancyModule
    {
        public TestClientRelayModule()
        {
            Get["/test"] = o =>
            {

                Console.WriteLine("Test Fired!!!");
                return new Response()
                {
                    StatusCode = HttpStatusCode.OK
                };
            };

            Get[@"/{name*}"] = parameters =>
            {

                Console.WriteLine("Catch all Fired!!!");
                return new Response()
                {
                    StatusCode = HttpStatusCode.OK
                };
            };


            Post[@"/{name*}", true] = async (parameters, ct) =>
            {
                Console.WriteLine("Post - Catch all Fired!!!");

                var body = await Context.Request.Body.ReadAsUtf8StringAsync();

                Console.WriteLine();
                Console.WriteLine(body);
                Console.WriteLine();
                return new Response()
                {
                    StatusCode = HttpStatusCode.OK
                };
            };
        }
    }
}
