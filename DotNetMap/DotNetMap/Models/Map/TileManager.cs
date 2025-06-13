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
        public List<TileProviderModel> TileProviders { get; set; } = new List<TileProviderModel>();

        public static TileManager LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TileManager>(json);
        }

        public TileModel GetTileByName(string name)
        {
            foreach (var provider in TileProviders)
            {
                foreach (var tile in provider.Tiles)
                {
                    if (tile.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return tile;
                }
            }
            return null;
        }
    }
}
