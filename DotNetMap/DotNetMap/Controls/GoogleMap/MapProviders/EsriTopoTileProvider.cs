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
    internal class EsriTopoTileProvider : GMapProvider, ITileProvider
    {
        public static readonly EsriTopoTileProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "OSM";

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

        public EsriTopoTileProvider()
        {
            MaxZoom = 19;
            MinZoom = 0;
            Copyright = string.Format("Tiles &copy; Esri &mdash; Esri, DeLorme, NAVTEQ, TomTom, Intermap, iPC, USGS, FAO, NPS, NRCAN, GeoBase, Kadaster NL, Ordnance Survey, Esri Japan, METI, Esri China (Hong Kong), and the GIS User Community");
        }

        static EsriTopoTileProvider()
        {
            Instance = new EsriTopoTileProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{zoom}/{pos.Y}/{pos.X}.png";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
