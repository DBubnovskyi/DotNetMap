using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetMap.Models.Map
{
    public class TileServer : ITileServer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tiles")]
        public List<ITileProvider> TileProviders { get; set; }

        public ITileProvider GetTileByName(string name)
        {
            return TileProviders.Find(tile => tile.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
