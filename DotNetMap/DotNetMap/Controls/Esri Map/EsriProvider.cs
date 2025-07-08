using DotNetMap.Models.Map;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Controls.Esri_Map
{
    public class EsriProvider : Basemap
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public int? MaxZoom { get; set; }
        public int? MinZoom { get; set; }
        public bool IsSatellite { get; set; }
        public bool IsHeightmap { get; set; }

        public EsriProvider(ITileProvider provider)
            : base(new WebTiledLayer(provider.Url))
        {
            Name = provider.Name ?? "Custom Esri Provider";
            Id = provider.Id;
            Url = provider.Url;
            Description = provider.Description;
            Copyright = provider.Copyright;
            MaxZoom = provider.MaxZoom;
            MinZoom = provider.MinZoom;
            IsSatellite = provider.IsSatellite;
            IsHeightmap = provider.IsHeightmap;
        }
    }
}
