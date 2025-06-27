using DotNetMap.Models.Map;
using DotNetMap.Processors;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Controls.GoogleMap
{
    public class GoogleTileProcessor
    {
        static GoogleTileProcessor()
        {
            CustomProvidersToGoogle();
        }

        public static List<GMapProvider> GMapProviders { get; set; } = new List<GMapProvider>();

        public static void CustomProvidersToGoogle()
        {
            TileManager tileManager = CustomTileProcessor.GetManager;

            foreach (ITileServer server in tileManager.TileServers)
            {
                foreach (ITileProvider tileProvider in server.TileProviders)
                {
                    var provider = new GMapCustom(tileProvider);
                    GMapProviders.Add(provider);
                }
            }
        }
    }
}
