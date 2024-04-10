import 'modern-normalize'
import './app.css'

import { clsx } from 'clsx'
import { Fragment, useEffect, useState } from 'react'
import useWebSocket, { ReadyState } from 'react-use-websocket'

interface IMediaData {
  Title: string
  Artist: string
  Album: string
  Position: number
  Duration: number
  Thumbnail: string
  IsPlaying: number
  IsPaused: number
  IsStopped: number
}

interface IPlaybackData {
  Position: number
  Duration: number
  IsPlaying: number
  IsPaused: number
  IsStopped: number
}

export default function App() {
  const [mediaData, setMediaData] = useState<IMediaData | null>(null)
  const [playbackData, setPlaybackData] = useState<IPlaybackData | null>(null)
  const { lastMessage, readyState } = useWebSocket(WS_URL, {
    retryOnError: true,
    reconnectAttempts: Number.MAX_SAFE_INTEGER,
    reconnectInterval: 1000,
    shouldReconnect: () => true
  })

  const noConnection = readyState !== ReadyState.OPEN
  const noConnectionOrPaused = noConnection || !mediaData?.IsPlaying

  useEffect(() => {
    if (!lastMessage) {
      return
    }

    setMediaData(JSON.parse(lastMessage.data))
  }, [lastMessage])

  useEffect(() => {
    if (!mediaData || noConnection) {
      return
    }

    let i = 0

    const interval = setInterval(() => {
      if (!mediaData) {
        return
      }

      if (mediaData.IsPlaying && mediaData.Position + i < mediaData.Duration) {
        i++
      }

      setPlaybackData({
        Position: mediaData.Position + i,
        Duration: mediaData.Duration,
        IsPlaying: mediaData.IsPlaying,
        IsPaused: mediaData.IsPaused,
        IsStopped: mediaData.IsStopped
      })
    }, 1000)

    return () => {
      clearInterval(interval)
    }
  }, [mediaData, noConnection])

  return (
    <div className="app">
      {mediaData && playbackData && (
        <Fragment>
          <div className={clsx('cover', noConnectionOrPaused && 'cover_paused')}>
            <img className="cover__image" src={`data:image/jpeg;base64,${mediaData.Thumbnail}`} />
            <div className="cover__overlay">
              <img
                className="cover__pause-icon"
                src={noConnection ? './broken-link-icon.svg' : './pause.svg'}
              />
            </div>
          </div>
          <div className="data">
            <span className="artist">{mediaData.Artist}</span>
            <span className="title">{mediaData.Title}</span>
            <span className="progress-text">
              {formatTime(playbackData.Position)} / {formatTime(playbackData.Duration)}
            </span>
          </div>
        </Fragment>
      )}
    </div>
  )
}

function formatTime(time: number): string {
  const hh = Math.floor(time / 3600)
  const mm = Math.floor((time % 3600) / 60)
  const ss = Math.floor(time % 60)

  const hours = hh.toString().padStart(2, '0')
  const minutes = mm.toString().padStart(2, '0')
  const seconds = ss.toString().padStart(2, '0')

  if (hh > 0) {
    return `${hours}:${minutes}:${seconds}`
  } else {
    return `${minutes}:${seconds}`
  }
}
