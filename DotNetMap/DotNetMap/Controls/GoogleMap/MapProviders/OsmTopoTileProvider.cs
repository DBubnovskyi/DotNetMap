using DotNetMap.Models.Map;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Controls.GoogleMap.MapProviders
{
    internal class OsmTopoTileProvider : GMapProvider, ITileProvider
    {
        public static readonly OsmTopoTileProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "OSM topo";

        public override PureProjection Projection => MercatorProjection.Instance;

        private GMapProvider[] _overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[1] { this };
                }

                return _overlays;
            }
        }

        ITileProvider ITileProvider.Instance => Instance;

        public OsmTopoTileProvider()
        {
            MaxZoom = 19;
            MinZoom = 0;
            Copyright = string.Format("OpenStreetMap Topo");
        }

        static OsmTopoTileProvider()
        {
            Instance = new OsmTopoTileProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"{GetServer()}.tile.opentopomap.org/{zoom}/{pos.Y}/{pos.X}.png";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }

        private static string GetServer()
        {
            string[] servers = { "a", "b", "c" };
            Random rnd = new Random();
            return servers[rnd.Next(servers.Length)];
        }
    }
}
