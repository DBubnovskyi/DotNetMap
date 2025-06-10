using DotNetMap.Models.Map;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;

namespace DotNetMap.Controls.GoogleMap.MapProviders
{
    public class Mt1google : GMapProvider, ITileProvider
    {
        public static readonly Mt1google Instance;

        public override Guid Id => Guid.NewGuid();

        public override string Name => "Sat Google mt1";

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

        public Mt1google()
        {
            MaxZoom = 18;
            MinZoom = 0;
            Copyright = "© Google";
        }

        static Mt1google()
        {
            Instance = new Mt1google();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = $"https://mt1.google.com/vt/lyrs=s&x={pos.X}&y={pos.Y}&z={zoom}";
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
