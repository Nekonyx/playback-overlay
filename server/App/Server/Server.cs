using NetCoreServer;

namespace PlaybackDataServer.App.Server
{
    public sealed class Server(string address, int port) : WsServer(address, port)
    {
        public event EventHandler<Session>? ClientConnected;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);

            OnClientConnected(session);

            return session;
        }

        private void OnClientConnected(Session e)
        {
            ClientConnected?.Invoke(this, e);
        }
    }
}