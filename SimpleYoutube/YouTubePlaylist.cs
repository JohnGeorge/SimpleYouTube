using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace SimpleYoutubePlaylist
{
	public class YouTubePlayList
	{
		private const string URL = "https://www.googleapis.com/youtube/v3/playlists";
		private string urlParameters;
		private List<RootObject.Item> playlist;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SimpleYoutube.YouTubePlayList"/> class.
		/// </summary>
		/// <param name="channelId">Channel identifier.</param>
		/// <param name="apiKey">API key.</param>
		/// <param name="maxResult">Maximum number of results to request.</param>
		public YouTubePlayList(string channelId, string apiKey, int maxResult)
		{
			this.urlParameters = "?part=snippet&channelId=" + channelId + "&maxResults=" + maxResult.ToString() + "&key=" + apiKey;
			this.playlist = GetPlaylists();
		}

		private List<RootObject.Item> GetPlaylists()
		{

			var client = new HttpClient();

			client.BaseAddress = new Uri(URL);

			client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json"));

			// Blocking call!
			HttpResponseMessage response = client.GetAsync(this.urlParameters).Result;  // Blocking call!

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
		/// Gets the playlist title, id, medium thumbnail src
		/// </summary>
		/// <returns>The playlist data.</returns>
		public List<PlaylistObject> GetPlaylistData()
		{
			var dataObject = new List<PlaylistObject>();

			foreach (var list in this.playlist)
			{
				dataObject.Add(new PlaylistObject
				{
					Title = list.snippet.title,
					ID = list.id,
					ThumbnailSrc = list.snippet.thumbnails.medium.url
				});
			}
			return dataObject;
		}

		public struct PlaylistObject
		{
			public string Title;
			public string ID;
			public string ThumbnailSrc;
		}

		private struct RootObject
		{
			public string kind { get; set; }
			public string etag { get; set; }
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

			public struct Snippet
			{
				public string publishedAt { get; set; }
				public string channelId { get; set; }
				public string title { get; set; }
				public string description { get; set; }
				public Thumbnails thumbnails { get; set; }
				public string channelTitle { get; set; }
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
