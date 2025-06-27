using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace DotNetMap.Controls.GoogleMap
{
    public partial class GoogleMapControl : UserControl
    {

        public GoogleMapControl()
        {
            InitializeComponent();
            InitializeMap();
            //Loaded += GoogleMapControl_Loaded;
            TileComboBox.ItemsSource = GoogleTileProcessor.GMapProviders;
            TileComboBox.DisplayMemberPath = "Name";
            TileComboBox.SelectedIndex = GoogleTileProcessor.GMapProviders.Any() ? 0 : -1;
        }

        public void InitializeMap()
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            GMapControl.Name = "mapView";
            GMapControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            GMapControl.VerticalAlignment = VerticalAlignment.Stretch;
            GMapControl.MapProvider = GMapProviders.GoogleMap;
            GMapControl.MinZoom = 2;
            GMapControl.MaxZoom = 17;
            GMapControl.Zoom = 2;
            GMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            GMapControl.CanDragMap = true;
            GMapControl.DragButton = MouseButton.Left;
        }

        private void TileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TileComboBox.SelectedItem is GMapProvider selectedProvider)
            {
                GMapControl.MapProvider = selectedProvider;
            }
        }
    }
}
