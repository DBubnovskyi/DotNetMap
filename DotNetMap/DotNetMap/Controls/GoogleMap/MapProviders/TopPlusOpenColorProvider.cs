using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;

namespace DotNetMap.Controls.GoogleMap.MapProviders
{
    internal class TopPlusOpenColorProvider : GMapProvider
    {
        public static readonly TopPlusOpenColorProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "TopPlusOpen Color";

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

        private TopPlusOpenColorProvider()
        {
            MaxZoom = 18;
            MinZoom = 0;
            Copyright = "Map data: © dl-de/by-2-0";
        }

        static TopPlusOpenColorProvider()
        {
            Instance = new TopPlusOpenColorProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"http://sgx.geodatenzentrum.de/wmts_topplus_open/tile/1.0.0/web/default/WEBMERCATOR/{zoom}/{pos.Y}/{pos.X}.png";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
