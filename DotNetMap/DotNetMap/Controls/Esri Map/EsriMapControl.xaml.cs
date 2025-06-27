using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotNetMap.Controls.EsriMap
{
    /// <summary>
    /// Interaction logic for EsriMapConstrol.xaml
    /// </summary>
    public partial class EsriMapConstrol : UserControl
    {
        public EsriMapConstrol()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ArcGISRuntimeEnvironment.ApiKey = ((TextBox)sender).Text;

            var uri = new Uri("https://server.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer");
            ArcGISMapImageLayer layer = new ArcGISMapImageLayer(uri);
            Task t = Task.Run(() => layer.LoadAsync());
            t.Wait();

            Map map = new Map(SpatialReference.Create(4326));
            map.OperationalLayers.Add(layer);
            t = Task.Run(() => map.LoadAsync());
            t.Wait();

            var scene = new Scene(BasemapStyle.ArcGISTopographic);

            const string elevationUrl = "https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer";
            var source = new ArcGISTiledElevationSource(new Uri(elevationUrl));
            var surface = new Surface();
            surface.ElevationSources.Add(source);
            scene.BaseSurface = surface;

            SceneView.Scene = scene;
        }
    }
}
