### Playback Overlay

for OBS or any other streaming application.

Uses native Windows tools to get data about the current song, so streaming apps like Spotify, Deezer, iTunes, or any playable song/video from YouTube or YouTube Music, VK, Yandex Music, and so on are supported.

![alt text](docs/hero.png)

### Requirements

- .NET 8.0 [[download here](https://dotnet.microsoft.com/download/)]
- Windows 10 (November Update or newer), Windows 11

### Usage

#### I. Download and extract the latest version

1. Download and extract **release-[numbers].zip** file from [releases page](https://github.com/Nekonyx/playback-overlay/releases/latest).
2. Run `PlaybackDataServer.exe` and leave it running.

#### II. Set up a streaming app

If it is OBS or Streamlabs, use the instructions below:

1. Create a new **Browser Source**
2. Make sure "Local File" checkbox is enabled.
3. Click "Browse" button and point to the `index.html` file.
4. Set **width** to `800` and **height** to `200`
5. Copy and paste **Custom CSS:**

```css
:root {
  --bg: transparent;
}
```

6. Done.

### Credits

[Pause icon created by Slidicon - Flaticon](https://www.flaticon.com/free-icons/pause)
