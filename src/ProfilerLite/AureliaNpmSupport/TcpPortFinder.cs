using System.Net;
using System.Net.Sockets;

namespace ProfilerLite.AureliaNpmSupport
{
    internal static class TcpPortFinder
    {
        public static int FindAvailablePort()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            try
            {
                return ((IPEndPoint) tcpListener.LocalEndpoint).Port;
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}