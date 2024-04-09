using NetCoreServer;

namespace PlaybackDataServer.App.Server
{
    public class Session(WsServer server) : WsSession(server)
    {
        public override void OnWsConnected(HttpRequest request)
        {
            Console.WriteLine($"WebSocket session with ID {Id} connected!");
        }

        public override void OnWsDisconnected()
        {
            Console.WriteLine($"WebSocket session with ID {Id} disconnected!");
        }
    }
}