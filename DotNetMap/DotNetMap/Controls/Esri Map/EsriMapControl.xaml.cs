using DotNetMap.Controls.Esri_Map;
using DotNetMap.Controls.GoogleMap;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            //TileComboBox.ItemsSource = Enum.GetValues(typeof(BasemapStyle)).Cast<BasemapStyle>();
            //TileComboBox.SelectedItem = BasemapStyle.ArcGISImageryStandard;

            TileComboBox.ItemsSource = EsriTileProcessor.EsriProviders;
            TileComboBox.DisplayMemberPath = "Name";
            TileComboBox.SelectedIndex = EsriTileProcessor.EsriProviders.Any() ? 0 : -1;

            ApiKey.Text = "AAPTxy8BH1VEsoebNVZXo8HurBBpdhidc3D37n5gyF1R1Lbhmuqo7pznsNwvbXFfue4RcCnzZAHEbYFRiU0-6YvCGhsW5B9Da3lYzbK8iKxqHiXVlwWCRY_l8Xqsj95TAOsX8w_tYOtqJUk4OUsjTz740JalmHF6_VqsdaBHsIttJkGMlrFemK4dxRs-47cgppdpwZLMCaT4_vLc6qGBlfPLMxLeJQcl7kjF8y8VRlvVuvg.AT1_hs42IvlQ";
            TextBox_TextChanged(this, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
        }

        private readonly SceneView _view = new SceneView();
        private readonly Scene _scene = new Scene();
        private Camera _camera;

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string apiKey = ApiKey.Text;
            bool valid = await IsApiKeyValidAsync(apiKey);
            if (valid)
            {

                ArcGISRuntimeEnvironment.ApiKey = apiKey;

                _scene.Basemap = (Basemap)TileComboBox.SelectedItem;

                const string elevationUrl = "https://elevation3d.arcgis.com/arcgis/rest/services/World_Physical_Map/MapServer/tile";
                var source = new ArcGISTiledElevationSource(new Uri(elevationUrl));
                var surface = new Surface();
                surface.ElevationSources.Add(source);
                _scene.BaseSurface = surface;

                _view.Scene = _scene;
                if (!MapContainer.Children.Contains(_view))
                    MapContainer.Children.Add(_view);


                _camera = new Camera(
                    latitude: 37.48013898957171,
                    longitude: 31.679512692563637,
                    altitude: 1225717.2166086873,
                    heading: 0,
                    pitch: 90 - 39.955294751008054,
                    roll: 0
                );

                _view.SetViewpointCamera(_camera);
                _view.ViewpointChanged += SetHeading;
            }
        }

        private void SetHeading(object sender, EventArgs e)
        {
            var camera = _view.Camera;
            var location = camera.Location;

            double latitude = location.Y;
            double longitude = location.X;
            double altitude = location.Z;
            double heading = camera.Heading;
            double pitch = 90.0 - camera.Pitch; 
            double roll = camera.Roll;

            CameraData.Text =
                $"Latitude: {latitude:F8}\n" +
                $"Longitude: {longitude:F8}\n" +
                $"Altitude: {altitude:F2} m\n" +
                $"Heading: {heading:F2}°\n" +
                $"Pitch: {pitch:F2}°\n" +
                $"Roll: {roll:F2}°";
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

        private void TileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_scene != null && TileComboBox.SelectedItem is Basemap selectedStyle)
            {
                _scene.Basemap = selectedStyle;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var camera = _view.Camera;
            var rotatedCamera = camera.RotateTo(0, camera.Pitch, camera.Roll);

            await _view.SetViewpointCameraAsync(rotatedCamera);
        }
    }
}
