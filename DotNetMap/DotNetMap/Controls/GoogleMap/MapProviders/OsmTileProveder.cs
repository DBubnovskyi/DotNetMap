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
    internal class OsmFrTileProvider : GMapProvider, ITileProvider
    {
        public static readonly OsmFrTileProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "OSM FR";

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

        public OsmFrTileProvider()
        {
            MaxZoom = 19;
            MinZoom = 0;
            Copyright = string.Format("OpenStreetMap");
        }
        static OsmFrTileProvider()
        {
            Instance = new OsmFrTileProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"https://{GetServer()}.tile.openstreetmap.org/{zoom}/{pos.X}/{pos.Y}.png";
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
