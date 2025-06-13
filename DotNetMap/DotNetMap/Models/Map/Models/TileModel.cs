using Newtonsoft.Json;

namespace DotNetMap.Map.Models
{
    public class TileModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("maxZoom")]
        public int MaxZoom { get; set; }

        [JsonProperty("minZoom")]
        public int MinZoom { get; set; }

        [JsonProperty("isSatellite")]
        public bool IsSatellite { get; set; }

        [JsonProperty("isHeigthmap")]
        public bool IsHeightmap { get; set; }
    }
}
