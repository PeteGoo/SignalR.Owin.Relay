using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Owin;

namespace SignalR.Owin.Relay.Common
{
    public class OwinContextDto
    {
        public string RequestBody { get; set; }
        public Dictionary<string, IEnumerable<string>> RequestHeaders { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public string RequestPathBase { get; set; }
        public string RequestProtocol { get; set; }
        public string RequestQueryString { get; set; }
        public string RequestScheme { get; set; }
        public string Version { get; set; }

        public Dictionary<string, object> ToOwinDictionary()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(RequestBody));

            return new Dictionary<string, object>
            {
                {"owin.RequestBody", stream},
                {"owin.RequestPath", RequestPath},
                {"owin.RequestHeaders", RequestHeaders.ToDictionary(x => x.Key, x => x.Value.ToArray())},
                {"owin.RequestMethod", RequestMethod},
                {"owin.RequestPathBase", RequestPathBase},
                {"owin.RequestProtocol", RequestProtocol},
                {"owin.RequestQueryString", RequestQueryString},
                {"owin.RequestScheme", RequestScheme},
                {"owin.ResponseHeaders", new Dictionary<string, string[]>()},
                {"owin.ResponseBody", new MemoryStream()},
                {"owin.Version", Version},
            };
        }
    }

    public static class OwinExtensions
    {
        public static async Task<OwinContextDto> ToDto(this NancyContext context)
        {
            var owinEnvironment = (IDictionary<string, object>)context.Items[NancyOwinHost.RequestEnvironmentKey];
            return new OwinContextDto
            {
                RequestBody = await context.Request.Body.ReadAsUtf8StringAsync(),
                RequestHeaders = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value),
                RequestMethod = owinEnvironment["owin.RequestMethod"] as string,
                RequestPath = owinEnvironment["owin.RequestPath"] as string,
                RequestPathBase = owinEnvironment["owin.RequestPathBase"] as string,
                RequestProtocol = owinEnvironment["owin.RequestProtocol"] as string,
                RequestQueryString = owinEnvironment["owin.RequestQueryString"] as string,
                RequestScheme = owinEnvironment["owin.RequestScheme"] as string,
                Version = owinEnvironment["owin.Version"] as string
            };
        }
    }
    
}