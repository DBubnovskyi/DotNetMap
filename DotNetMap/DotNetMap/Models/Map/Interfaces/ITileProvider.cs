using System;

namespace DotNetMap.Models.Map
{
    public interface ITileProvider
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string Url { get; set; }

        string Copyright { get; set; }

        int MaxZoom { get; set; }

        int MinZoom { get; set; }

        bool IsSatellite { get; set; }

        bool IsHeightmap { get; set; }

        ITileProvider Instance { get; }
    }
}
