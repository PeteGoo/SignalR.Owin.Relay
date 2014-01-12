using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Owin.Relay.Common
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsUtf8StringAsync(this Stream stream)
        {
            //var sb = new StringBuilder();
            //var buffer = new byte[8000];
            //var read = 0;

            //read = await stream.ReadAsync(buffer, 0, buffer.Length);
            //while (read > 0)
            //{
            //    sb.Append(Encoding.UTF8.GetString(buffer));
            //    buffer = new byte[8000];
            //    read = await stream.ReadAsync(buffer, 0, buffer.Length);
            //}

            //return sb.ToString();

            return new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();
        } 
    }
}