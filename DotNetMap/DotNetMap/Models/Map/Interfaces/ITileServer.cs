using System.Collections.Generic;

namespace DotNetMap.Models.Map
{
    public interface ITileServer
    {
        string Name { get; }

        List<ITileProvider> TileProviders { get; set; }
    }
}
