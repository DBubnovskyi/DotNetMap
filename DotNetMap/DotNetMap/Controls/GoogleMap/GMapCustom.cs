using DotNetMap.Models.Map;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;

namespace DotNetMap.Controls.GoogleMap
{
    internal class GMapCustom : GMapProvider, ITileProvider
    {
        const int _defaultMaxZoom = 19;
        public static readonly GMapCustom Instance;
        ITileProvider ITileProvider.Instance => Instance;

        private Guid _guid = Guid.NewGuid();
        public override Guid Id => _guid;
        Guid ITileProvider.Id { get => _guid; set => _guid = value; }

        private string _name = "Custom provider";
        public override string Name => _name;
        string ITileProvider.Name { get => _name; set => _name = value; }

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

        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsSatellite { get; set; }
        public bool IsHeightmap { get; set; }
        string ITileProvider.Copyright { get => Copyright; set => Copyright = value; }
        int ITileProvider.MaxZoom { get => MaxZoom?? _defaultMaxZoom; set => MaxZoom = value; }
        int ITileProvider.MinZoom { get => MinZoom; set => MinZoom = value; }

        public GMapCustom(ITileProvider provider)
        {
            MaxZoom = provider.MaxZoom;
            MinZoom = provider.MinZoom;
            Copyright = provider.Copyright ?? string.Empty;
            Url = provider.Url;
            Description = provider.Description ?? string.Empty;
            IsSatellite = provider.IsSatellite;
            IsHeightmap = provider.IsHeightmap;
            _guid = provider.Id;
            _name = provider.Name ?? "Custom provider";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = Url?
                    .Replace("{z}", $"{zoom}")
                    .Replace("{x}", $"{pos.X}")
                    .Replace("{y}", $"{pos.Y}")
                    .Replace("{s}", GetServer());
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
