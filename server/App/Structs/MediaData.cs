using System.Runtime.Serialization;

namespace PlaybackDataServer.App.Structs;

[DataContract]
public struct MediaData
{
    public string Title { get; set; }

    public string Artist { get; set; }

    public string Album { get; set; }

    public int Position { get; set; }

    public int Duration { get; set; }
    
    public string Thumbnail { get; set; }
    
    public bool IsPlaying { get; set; }
    
    public bool IsPaused { get; set; }
    
    public bool IsStopped { get; set; }
}