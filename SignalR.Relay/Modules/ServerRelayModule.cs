using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Nancy;
using Microsoft.Owin;
using Nancy.Owin;
using Newtonsoft.Json;
using SignalR.Owin.Relay.Common;

namespace SignalR.Relay.Modules
{
    public class ServerRelayModule : NancyModule
    {
        public ServerRelayModule()
        {
            //Get[@"/(.*)/(.*)"] = parameters => new Response()
            //{
            //    StatusCode = HttpStatusCode.OK
            //};

            Get[@"/{name*}"] = parameters =>
            {


                SendContextToSignalR(Context);

                return new Response()
                {
                    StatusCode = HttpStatusCode.OK
                };
            };

            Post[@"/{name*}"] = parameters =>
            {


                SendContextToSignalR(Context);

                return new Response()
                {
                    StatusCode = HttpStatusCode.OK
                };
            };
        }




        private async Task SendContextToSignalR(NancyContext context)
        {
            var connection = GlobalHost.ConnectionManager.GetConnectionContext<RelayConnection>();
            if (connection == null)
            {
                return;
            }

            var body = (string)null;

            var owinEnvironment = (IDictionary<string, object>)Context.Items[NancyOwinHost.RequestEnvironmentKey];

            var owinContextDto = new OwinContextDto
            {
                RequestBody = (await context.Request.Body.ReadAsUtf8StringAsync()).Trim(), 
                RequestHeaders = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value), 
                RequestMethod = owinEnvironment["owin.RequestMethod"] as string, 
                RequestPath = owinEnvironment["owin.RequestPath"] as string, 
                RequestPathBase = owinEnvironment["owin.RequestPathBase"] as string, 
                RequestProtocol = owinEnvironment["owin.RequestProtocol"] as string, 
                RequestQueryString = owinEnvironment["owin.RequestQueryString"] as string, 
                RequestScheme = owinEnvironment["owin.RequestScheme"] as string, 
                Version = owinEnvironment["owin.Version"] as string
            };

            await connection.Connection.Broadcast(owinContextDto);
        }
    }
}