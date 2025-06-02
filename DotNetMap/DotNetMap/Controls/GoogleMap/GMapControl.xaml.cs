using DotNetMap.Controls.GoogleMap.MapProviders;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace DotNetMap.Controls.GoogleMap
{
    /// <summary>
    /// Interaction logic for GMapControl.xaml
    /// </summary>
    public partial class GMapControl : UserControl
    {
        public GMapControl()
        {
            InitializeComponent();

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            var map = new GMap.NET.WindowsPresentation.GMapControl();
            map.Name = "mapView";
            map.HorizontalAlignment = HorizontalAlignment.Stretch;
            map.VerticalAlignment = VerticalAlignment.Stretch;
            map.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            map.MapProvider = TopoMapLayerProvider.Instance;
            map.MinZoom = 2;
            map.MaxZoom = 17;
            map.Zoom = 2;
            map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;
            Content.Children.Add(map);
        }
    }
}
