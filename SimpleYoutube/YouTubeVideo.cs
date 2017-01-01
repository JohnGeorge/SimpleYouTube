using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace SimpleYoutubePlaylist
{
	public class YouTubeVideo
	{
		private const string URL = "https://www.googleapis.com/youtube/v3/playlists";
		private string urlParameters;
		private List<RootObject.Item> videos;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SimpleYoutube.YouTubeVideo"/> class.
		/// </summary>
		/// <param name="playListId">Playlist identifier.</param>
		/// <param name="apiKey">API key.</param>
		/// <param name="maxResult">Maximum number of results to request.</param>
		public YouTubeVideo(string playListId, string apiKey, int maxResult)
		{
			this.urlParameters = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=" + maxResult + "&playlistId=" + playListId + "&key=" + apiKey;
			this.videos = GetVideos();
		}

		private List<RootObject.Item> GetVideos()
		{

			var client = new HttpClient();

			client.BaseAddress = new Uri(URL);

			client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json"));

			// Blocking call!
			HttpResponseMessage response = client.GetAsync(this.urlParameters).Result;

			if (response.IsSuccessStatusCode)
			{

				var json = response.Content.ReadAsStringAsync().Result;

				var jsonToObject = JObject.Parse(json);

				var itemsToJArray = (JArray)jsonToObject["items"];

				var listOfItems = itemsToJArray.ToObject<List<RootObject.Item>>();


				return listOfItems;
			}
			else
			{
				throw (new WebException("Your request returned " + (int)response.StatusCode + ", " + response.ReasonPhrase));
			}
		}

		/// <summary>
		/// Gets the video title, id, medium thumbnail src
		/// </summary>
		/// <returns>The video data.</returns>
		public List<VideoObject> GetVideoData()
		{
			var dataObject = new List<VideoObject>();

			foreach (var video in this.videos)
			{
				dataObject.Add(new VideoObject
				{
					Title = video.snippet.title,
					ID = video.snippet.resourceId.videoId,
					ThumbnailSrc = video.snippet.thumbnails.medium.url
				});
			}
			return dataObject;
		}

		public struct VideoObject
		{
			public string Title;
			public string ID;
			public string ThumbnailSrc;
		}

		public struct RootObject
		{
			public string kind { get; set; }
			public string etag { get; set; }
			public string nextPageToken { get; set; }
			public List<Item> items { get; set; }

			public struct Medium
			{
				public string url { get; set; }
				public int width { get; set; }
				public int height { get; set; }
			}


			public struct Thumbnails
			{
				public Medium medium { get; set; }
			}

			public struct ResourceId
			{
				public string kind { get; set; }
				public string videoId { get; set; }
			}

			public struct Snippet
			{
				public string publishedAt { get; set; }
				public string channelId { get; set; }
				public string title { get; set; }
				public string description { get; set; }
				public Thumbnails thumbnails { get; set; }
				public string channelTitle { get; set; }
				public string playlistId { get; set; }
				public int position { get; set; }
				public ResourceId resourceId { get; set; }
			}

			public struct Item
			{
				public string kind { get; set; }
				public string etag { get; set; }
				public string id { get; set; }
				public Snippet snippet { get; set; }
			}
		}
	}
}
