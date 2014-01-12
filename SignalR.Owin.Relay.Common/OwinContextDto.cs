using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    
}