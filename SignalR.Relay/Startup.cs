using Microsoft.AspNet.SignalR;
using Owin;

namespace SignalR.Relay
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<RelayConnection>("/relay")
               .UseNancy()
               ;
        }
    }
}