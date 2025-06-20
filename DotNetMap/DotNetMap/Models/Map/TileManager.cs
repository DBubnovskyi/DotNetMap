using System;
using System.Collections.Generic;
using DotNetMap.Models.Map;
using GMap.NET.Internals;
using Newtonsoft.Json;

namespace DotNetMap.Map.Models
{
    public class TileManager
    {
        [JsonProperty("tileProviders")]
        public List<TileServer> TileProviders { get; set; } = new List<TileServer>();

        public static TileManager LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TileManager>(json);
        }
    }
}
