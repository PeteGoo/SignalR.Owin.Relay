using Owin;

namespace RelayClientTest
{
    public class NancyStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseNancy();
        }

    }
}
