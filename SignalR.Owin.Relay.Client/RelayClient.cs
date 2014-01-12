using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Owin;
using SignalR.Owin.Relay.Common;

namespace SignalR.Owin.Relay.Client
{
    public class RelayClient : IDisposable
    {
        private readonly string _url;
        private readonly TestServer _server;
        private Connection _connection;

        public static RelayClient Create<TStartup>(string relayUrl)
        {
            var server = TestServer.Create<TStartup>();
            return new RelayClient(server, relayUrl);
        }

        public static RelayClient Create(Action<IAppBuilder> startup, string relayUrl)
        {
            var server = TestServer.Create(startup);
            return new RelayClient(server, relayUrl);
        }

        private RelayClient(TestServer server, string url)
        {
            _url = url;
            _server = server;
        }

        public async Task<IDisposable> Start()
        {
            if (_connection != null)
            {
                return new Disposable(Stop);
            }

            _connection = CreateConnection(_url);

            _connection.StateChanged += change =>
            {
                if (change.NewState == ConnectionState.Disconnected)
                {
                    OnDisconnected();
                }
            };

            _connection.Received += s =>
            {
                if (s == null)
                {
                    return;
                }
                var context = JsonConvert.DeserializeObject<OwinContextDto>(s.Trim());

                _server.Invoke(context.ToOwinDictionary());
            };

            await _connection.Start();


            return new Disposable(Stop);
        }

        protected virtual Connection CreateConnection(string url)
        {
            return new Connection(url);
        }


        private async void OnDisconnected()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await _connection.Start();

            }
            catch (Exception ex)
            {
                try
                {
                    _connection.Trace(TraceLevels.Events, ex.Message);
                }
                catch (Exception)
                {
                    // For some reason the Trace method can throw :(
                }
            }
        }

        private class Disposable : IDisposable 
        {
            private readonly Action _dispose;
            private bool _disposed;

            public Disposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                if (_dispose != null && !_disposed)
                {
                    _dispose();
                    _disposed = true;
                }
            }
        }

        public void Stop()
        {
            if (_connection != null)
            {
                _connection.Stop();
                _connection.Dispose();
                _connection = null;
            }
        }


        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }
    }
}