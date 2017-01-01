# SimpleYouTube
NuGet package for simple access to stripped down YouTube playlist and video data

To Retrieve YouTube playlist data create an instance of YouTubePlaylist and provide necessary data.
```csharp
var playlists = new YoutubePlaylist(channelId, apiKey, maxResult).GetPlaylistData();
```
This will return a List of playlist objects

```csharp
struct PlaylistObject
		{
			public string Title;
			public string ID;
			public string ThumbnailSrc;
		}
```

To Retrieve YouTube video data create an instance of YouTubeVideo and provide necessary data.
```csharp
var videos = new YoutubeVideo(playlistId, apiKey, maxResult).GetVideoData();
```
This will return a List of video objects

```csharp
struct VideoObject
		{
			public string Title;
			public string ID;
			public string ThumbnailSrc;
		}
```

Example:
```csharp
using System;
using System.Collections.Generic;
using SimpleYoutubePlaylist;

class Program
{ 
  static void Main(string[] args)
  {
   var channelId = "SOME_CHANNEL_ID";
			var apiKey = "YOUR_API_KEY";

			var playlist = new YouTubePlayList(channelId, apiKey, 10).GetPlaylistData();


			foreach (var list in playlist)
			{
				Console.WriteLine(list.Title);
        
				var videos = new YouTubeVideo(list.ID, apiKey, 3).GetVideoData();
		
				foreach (var video in videos)
				{
					Console.WriteLine(video.Title);
				}
				
			}

  }
}
```
#Testing
