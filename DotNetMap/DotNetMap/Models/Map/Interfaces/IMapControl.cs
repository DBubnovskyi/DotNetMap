using System.Collections.Generic;

namespace DotNetMap.Models.Map
{
    public interface IMapControl
    {
        /// <summary>
        /// Initializes the map control.
        /// </summary>
        void InitializeMap();

        /// <summary>
        /// Zooms in the map.
        /// </summary>
        void ZoomIn();

        /// <summary>
        /// Zooms out the map.
        /// </summary>
        void ZoomOut();

        /// <summary>
        /// Pans the map to the specified coordinates.
        /// </summary>
        /// <param name="latitude">The latitude to pan to.</param>
        /// <param name="longitude">The longitude to pan to.</param>
        void PanTo(double latitude, double longitude);

        List<ITileProvider> Tiles { get; set; }
    }
}
