using DotNetMap.Controls.GoogleMap;
using DotNetMap.Models.Map;
using DotNetMap.Processors;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Controls.Esri_Map
{
    public class EsriTileProcessor
    {
        static EsriTileProcessor()
        {
            CustomProvidersToEsri();
        }

        public static List<EsriProvider> EsriProviders { get; set; } = new List<EsriProvider>();

        public static void CustomProvidersToEsri()
        {
            TileManager tileManager = CustomTileProcessor.GetManager;

            foreach (ITileServer server in tileManager.TileServers)
            {
                foreach (ITileProvider tileProvider in server.TileProviders)
                {
                    tileProvider.Url = tileProvider.Url
                        .Replace("{s}", "a")
                        .Replace("{z}", "{level}")
                        .Replace("{x}", "{col}")
                        .Replace("{y}", "{row}");
                    var provider = new EsriProvider(tileProvider);
                    EsriProviders.Add(provider);
                }
            }
        }
    }
}
