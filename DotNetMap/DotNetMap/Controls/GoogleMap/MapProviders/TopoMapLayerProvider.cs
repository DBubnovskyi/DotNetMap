using DotNetMap.Models.Map;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;

namespace DotNetMap.Controls.GoogleMap.MapProviders
{
    internal class TopoMapLayerProvider : GMapProvider, ITileProvider
    {
        public static readonly TopoMapLayerProvider Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "TopoMap Layer";

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

        public TopoMapLayerProvider()
        {
            MaxZoom = 18;
            MinZoom = 0;
            Copyright = "Tiles © AWS Open Data";
        }

        static TopoMapLayerProvider()
        {
            Instance = new TopoMapLayerProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"https://elevation-tiles-prod.s3.amazonaws.com/terrarium/{zoom}/{pos.X}/{pos.Y}.png";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
