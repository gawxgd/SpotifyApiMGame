using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace spotifyAPIgae
{
    public class ExternalUrls
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }

        public ExternalUrls Clone()
        {
            return (ExternalUrls)MemberwiseClone();
        }
    }

    public class Followers
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        public Followers Clone()
        {
            return (Followers)MemberwiseClone();
        }
    }

    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        public Image Clone()
        {
            return (Image)MemberwiseClone();
        }
    }

    public class SpotifyUser : ICloneable
    {
        public string ImageUrl => Images?.FirstOrDefault()?.Url;
        public int? ImageWidth => Images?.FirstOrDefault()?.Width;
        public int? ImageHeight => Images?.FirstOrDefault()?.Height;

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("followers")]
        public Followers Followers { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public object Clone()
        {
            // Serialize the current object to JSON
            var json = JsonConvert.SerializeObject(this);
            // Deserialize the JSON back to a new instance
            return JsonConvert.DeserializeObject<SpotifyUser>(json);
        }
    }
}
