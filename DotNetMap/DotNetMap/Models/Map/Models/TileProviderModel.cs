using GMap.NET.Internals;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetMap.Map.Models
{
    public class TileProviderModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tiles")]
        public List<TileModel> Tiles { get; set; }
    }
}
