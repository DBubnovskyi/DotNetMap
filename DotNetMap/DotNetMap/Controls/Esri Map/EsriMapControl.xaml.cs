using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
            ApiKey.Text = "AAPTxy8BH1VEsoebNVZXo8HurBBpdhidc3D37n5gyF1R1Lbhmuqo7pznsNwvbXFfue4RcCnzZAHEbYFRiU0-6YvCGhsW5B9Da3lYzbK8iKxqHiXVlwWCRY_l8Xqsj95TAOsX8w_tYOtqJUk4OUsjTz740JalmHF6_VqsdaBHsIttJkGMlrFemK4dxRs-47cgppdpwZLMCaT4_vLc6qGBlfPLMxLeJQcl7kjF8y8VRlvVuvg.AT1_hs42IvlQ";
        }
        SceneView _view;
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string apiKey = ApiKey.Text;
            bool valid = await IsApiKeyValidAsync(apiKey);
            if (valid)
            {

                ArcGISRuntimeEnvironment.ApiKey = apiKey;

                var scene = new Scene(BasemapStyle.ArcGISChartedTerritoryBase);

                const string elevationUrl = "https://elevation3d.arcgis.com/arcgis/rest/services/World_Physical_Map/MapServer/tile";
                var source = new ArcGISTiledElevationSource(new Uri(elevationUrl));
                var surface = new Surface();
                surface.ElevationSources.Add(source);
                scene.BaseSurface = surface;

                _view = new SceneView
                {
                    Scene = scene,
                };
                _view.DragEnter += SetHeading;

                MapContainer.Children.Add(_view);

                var camera = new Camera(
                    latitude: 37.48013898957171,
                    longitude: 31.679512692563637,
                    altitude: 1225717.2166086873,
                    heading: 0,
                    pitch: 90 - 39.955294751008054, // конвертація tilt → pitch
                    roll: 0
                );

                _view.SetViewpointCamera(camera);
            }
        }

        private void SetHeading(object sender, DragEventArgs e)
        {
            CameraData.Text = $"Heading: {_view.Camera.Heading}\n" +
                $"m: {_view.Camera.Location.M}";
        }

        private async Task<bool> IsApiKeyValidAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Dispatcher.Invoke(() =>
                {
                    IsKeyValid.Text = "Invalid";
                    IsKeyValid.Background = new SolidColorBrush(Colors.OrangeRed);
                });
                return false;
            }

            const string testUrl = "https://www.arcgis.com/sharing/rest/portals/self?f=json&token=";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(testUrl + apiKey);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        if (!json.Contains("\"error\""))
                        {
                            Dispatcher.Invoke(() =>
                            {
                                IsKeyValid.Text = "Ok";
                                IsKeyValid.Background = new SolidColorBrush(Colors.GreenYellow);
                            });
                            return true;
                        }
                        else
                        {
                            Dispatcher.Invoke(() =>
                            {
                                IsKeyValid.Text = "Invalid";
                                IsKeyValid.Background = new SolidColorBrush(Colors.OrangeRed);
                            });
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    Dispatcher.Invoke(() =>
                    {
                        IsKeyValid.Text = "Error";
                        IsKeyValid.Background = new SolidColorBrush(Colors.Red);
                    });
                    return false;
                }
                return false;
            }
        }
    }
}
