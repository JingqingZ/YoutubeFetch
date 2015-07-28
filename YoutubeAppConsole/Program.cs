using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YoutubeAppConsole
{
    class YoutubeFetch
    {
        //search?part=snippet&q=IBM+Security&type=channel
        private string key = "AIzaSyCVoYXZZHaRNqnJw6pINn9PG3wly3_xNYY";
        private string baseURL = "https://www.googleapis.com/youtube/v3/";

        public class URLInfo
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class ThumbNailsInfo
        {
            public URLInfo medium { get; set; }
            public URLInfo high { get; set; }
        }

        public class ResourceIdInfo
        {
            public string kind { get; set; }
            public string videoId { get; set; }
        }

        public class SnippetInfo
        {
            public string publishedAt { get; set; }
            public string channelId { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public ThumbNailsInfo thumbnails { get; set; }
            public string channelTitle { get; set; }
            public string liveBroadcastContent { get; set; }
            public string playlistId { get; set; }
            public int position { get; set; }
            public ResourceIdInfo resourceId { get; set; }
        }

        public class RelatedPlaylistsInfo
        {
            public string likes { get; set; }
            public string uploads { get; set; }
        }

        public class ContentDetailsInfo
        {
            public RelatedPlaylistsInfo relatedPlaylists { get; set; }
            public string googlePlusUserId { get; set; }
            public string videoId { get; set; }
        }

        public class ChannelIdInfo
        {
            public string kind { get; set; }
            public string channelId { get; set; }
        }

        public class ChannelItemInfo
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public ChannelIdInfo id { get; set; }
            public SnippetInfo snippet { get; set; }
            public ContentDetailsInfo contentDetails { get; set; }
        }

        public class ItemInfo
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public SnippetInfo snippet { get; set; }
            public ContentDetailsInfo contentDetails { get; set; }
        }

        public class PageNumInfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }

        public class ChannelInfo
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string nextPageToken { get; set; }
            public PageNumInfo pageInfo { get; set; }
            public ChannelItemInfo[] items { get; set; }
        }

        public class PlayInfo
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string nextPageToken { get; set; }
            public PageNumInfo pageInfo { get; set; }
            public ItemInfo[] items { get; set; }
        }

        public string requestYoutubeInfo(string yURL)
        {
            WebRequest yRequest = WebRequest.Create(yURL);
            yRequest.Method = "GET";
            //Console.WriteLine(yURL);

            Stream yStream;
            yStream = yRequest.GetResponse().GetResponseStream();
            StreamReader yReader = new StreamReader(yStream);
            string yResponse = "";
            string yLine = "";
            while (yLine != null)
            {
                yLine = yReader.ReadLine();
                if (yLine != null)
                    yResponse += yLine;
            }
            //Console.WriteLine(yResponse);
            return yResponse;
        }

        public PlayInfo getVideos(string playlistId)
        {
            string yURL = baseURL + "playlistItems" + "?";
            yURL += "part=" + "contentDetails" + ",snippet" + "&";
            yURL += "playlistId=" + playlistId + "&";
            yURL += "maxResults=" + "32" + "&";
            yURL += "key=" + key;

            string yResponse = requestYoutubeInfo(yURL);
            PlayInfo yJson = JsonConvert.DeserializeObject<PlayInfo>(yResponse);
            return yJson;
        }

        public PlayInfo getPlaylists(string channelId)
        {
            string yURL = baseURL + "channels" + "?";
            yURL += "part=" + "contentDetails" + ",snippet" + "&";
            yURL += "id=" + channelId + "&";
            yURL += "key=" + key;

            string yResponse = requestYoutubeInfo(yURL);
            PlayInfo yJson = JsonConvert.DeserializeObject<PlayInfo>(yResponse);
            //Console.WriteLine(yJson.items[0].contentDetails.relatedPlaylists.uploads);
            return yJson;
        }

        public ChannelInfo getChannelId(string channelName)
        {
            string yURL = baseURL + "search" + "?";
            yURL += "part=" + "snippet" + "&";
            yURL += "q=" + channelName.Replace(" ", "+") + "&";
            yURL += "type=" + "channel" + "&";
            yURL += "maxResults=" + "3" + "&";
            yURL += "key=" + key;
            
            string yResponse = requestYoutubeInfo(yURL);
            ChannelInfo yJson = JsonConvert.DeserializeObject<ChannelInfo>(yResponse);
            return yJson;
        }
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: *.exe channelName");
                return;
            }
            YoutubeFetch yf = new YoutubeFetch();
            ChannelInfo channelJson = yf.getChannelId(args[0]);
            if (channelJson.items.Length <= 0)
            {
                Console.WriteLine("Error! Channel not found!");
                return;
            }
            Console.WriteLine("Channel: " + channelJson.items[0].snippet.title);
            PlayInfo playlistJson = yf.getPlaylists(channelJson.items[0].id.channelId);
            if (playlistJson.items.Length <= 0)
            {
                Console.WriteLine("Error! Playlist not found!");
            }
            PlayInfo videoJson = yf.getVideos(playlistJson.items[0].contentDetails.relatedPlaylists.uploads);
            //Console.WriteLine(videoJson.items[0].snippet.title);
            Console.WriteLine("Videos List:");
            for ( int i = 0 ; i < videoJson.items.Length ; i++)
            {
                Console.WriteLine(i + ": " + videoJson.items[i].snippet.title);
            }
        }
    }
}
