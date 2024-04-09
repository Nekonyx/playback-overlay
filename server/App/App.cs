using System.Text.Json;
using NPSMLib;
using PlaybackDataServer.App.Extensions;
using PlaybackDataServer.App.Server;
using PlaybackDataServer.App.Structs;

namespace PlaybackDataServer.App
{
    public class App : IDisposable
    {
        private const string Address = "0.0.0.0";
        private const int Port = 9764;

        private readonly Server.Server _server = new(Address, Port);
        private readonly NowPlayingSessionManager _npSessionManager = new();

        private NowPlayingSession? _npSession;
        private MediaPlaybackDataSource? _playback;

        public void Start()
        {
            Console.WriteLine("Starting server...");
            _server.Start();

            Console.WriteLine("Starting Now Playing Session Manager...");
            ChangeSession(_npSessionManager.CurrentSession);

            _server.ClientConnected += OnServerClientConnected;
            _npSessionManager.SessionListChanged += OnNowPlayingSessionListChanged;
        }

        public void Stop()
        {
            _server.ClientConnected -= OnServerClientConnected;
            _npSessionManager.SessionListChanged -= OnNowPlayingSessionListChanged;

            if (_playback is not null)
            {
                _playback.MediaPlaybackDataChanged -= OnPlaybackDataChanged;
            }

            Console.WriteLine("Stopping server...");
            _server.Stop();
        }

        public void Dispose()
        {
            Stop();
            _server.Dispose();
            GC.SuppressFinalize(this);
        }

        private void ChangeSession(NowPlayingSession session)
        {
            if (_playback is not null)
            {
                _playback.MediaPlaybackDataChanged -= OnPlaybackDataChanged;
            }

            _npSession = session;
            _playback = _npSession.ActivateMediaPlaybackDataSource();
            _playback.MediaPlaybackDataChanged += OnPlaybackDataChanged;
        }

        private void SendMediaData()
        {
            if (_playback is null)
            {
                Console.WriteLine("No active playback data source.");
                return;
            }

            var info = _playback.GetMediaObjectInfo();
            var timeline = _playback.GetMediaTimelineProperties();

            Console.WriteLine(
                $"Now playing: {info.Artist} - {info.Title} ({timeline.Position.Format()}/{timeline.EndTime.Format()})");

            if (_server.ConnectedSessions == 0)
            {
                return;
            }

            var state = _playback.GetMediaPlaybackInfo();
            var thumbnail = _playback.GetThumbnailStream();

            var data = new MediaData
            {
                Title = info.Title,
                Artist = info.Artist,
                Album = info.AlbumTitle,
                Position = (int)timeline.Position.TotalSeconds,
                Duration = (int)timeline.EndTime.TotalSeconds,
                Thumbnail = thumbnail != null ? thumbnail.ToBase64() : string.Empty,
                IsPlaying = state.PlaybackState == MediaPlaybackState.Playing,
                IsPaused = state.PlaybackState == MediaPlaybackState.Paused,
                IsStopped = state.PlaybackState == MediaPlaybackState.Stopped
            };

            var json = JsonSerializer.Serialize(data);

            Console.WriteLine($"Sending media data ({json.Length} bytes)...");
            _server.MulticastText(json);
        }

        private void OnNowPlayingSessionListChanged(object? sender, NowPlayingSessionManagerEventArgs e)
        {
            ChangeSession(_npSessionManager.CurrentSession);
        }

        private void OnServerClientConnected(object? sender, Session e)
        {
            SendMediaData();
        }

        private void OnPlaybackDataChanged(object? sender, MediaPlaybackDataChangedArgs e)
        {
            SendMediaData();
        }
    }
}