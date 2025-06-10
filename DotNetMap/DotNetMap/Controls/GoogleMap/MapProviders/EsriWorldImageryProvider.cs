using DotNetMap.Models.Map;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;

namespace DotNetMap.Controls.GoogleMap.MapProviders
{
    internal class EsriWorldImageryProvider : GMapProvider, ITileProvider
    {

        public static readonly EsriWorldImageryProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "Esri World Imagery";

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

        public EsriWorldImageryProvider()
        {
            MaxZoom = 19;
            MinZoom = 0;
            Copyright = "Tiles © Esri — Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community";
        }

        static EsriWorldImageryProvider()
        {
            Instance = new EsriWorldImageryProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{zoom}/{pos.Y}/{pos.X}";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
