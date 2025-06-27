using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetMap.Models.Map
{
    public class TileServer : ITileServer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tiles")]
        public List<TileProvider> Tile { get; set; }

        private List<ITileProvider> _tiles;

        public List<ITileProvider> TileProviders { 
            get
            {
                if(_tiles == null && Tile != null)
                {
                    _tiles = new List<ITileProvider>(Tile);
                }
                return _tiles;
            }
            set { _tiles = value; }
         }

        public ITileProvider GetTileByName(string name)
        {
            return TileProviders.Find(tile => tile.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
