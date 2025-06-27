using System.Collections.Generic;
using DotNetMap.Models.Map.Interfaces;
using DotNetMap.Processors;
using GMap.NET.Internals;
using Newtonsoft.Json;

namespace DotNetMap.Models.Map
{
    public class TileManager : ITileManager
    {
        //https://leaflet-extras.github.io/leaflet-providers/preview/
        [JsonProperty("tileServers")]
        public List<TileServer> Servers { get; set; }

        private List<ITileServer> _providers;

        public List<ITileServer> TileServers
        {
            get
            {
                if (_providers == null && Servers != null)
                {
                    _providers = new List<ITileServer>(Servers);
                }
                return _providers;
            }
            set { _providers = value; }
        }
    }
}
