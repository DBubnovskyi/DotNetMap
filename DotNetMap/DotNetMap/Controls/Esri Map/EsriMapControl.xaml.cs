using DotNetMap.Controls.Esri_Map;
using DotNetMap.Controls.GoogleMap;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.UI.GeoAnalysis;
using System;
using System.Collections.Generic;
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
            var style = Enum.GetValues(typeof(BasemapStyle))
                .Cast<BasemapStyle>()
                .Select(s => new { Name = s.ToString(), Value = s });

            List<dynamic> combined = new List<dynamic>();
            combined.AddRange(EsriTileProcessor.EsriProviders);
            combined.AddRange(style);
            TileComboBox.DisplayMemberPath = "Name";
            TileComboBox.ItemsSource = combined;
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

                if (TileComboBox.SelectedItem.GetType() == typeof(BasemapStyle))
                {
                    _view.Scene = new Scene((BasemapStyle)TileComboBox.SelectedItem);
                }

                if (TileComboBox.SelectedItem.GetType() == typeof(Basemap))
                {
                    _scene.Basemap = (Basemap)TileComboBox.SelectedItem;
                }

                const string elevationUrl = "https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer";
                var source = new ArcGISTiledElevationSource(new Uri(elevationUrl));
                var surface = new Surface();
                surface.ElevationSources.Add(source);
                _scene.BaseSurface = surface;

                _view.Scene = _scene;
                if (!MapContainer.Children.Contains(_view))
                    MapContainer.Children.Add(_view);

                _camera = new Camera(
                    latitude: 43.9,
                    longitude: 31.8,
                    altitude: 1070000,
                    heading: 0,
                    pitch: 90 - 70,
                    roll: 0
                );

                _view.SetViewpointCamera(_camera);
                //_view.ViewpointChanged += SetHeading;
                AddViewshedToSceneView(_view);
                AddGeodeticBufferToSceneView(_view);
            }
        }

        private void AddViewshedToSceneView(SceneView view, double latitude = 44.4511, double longitude = 34.0528, double altitude = 1234.2)
        {
            // Створення точки спостерігача
            var observerLocation = new MapPoint(longitude, latitude, altitude, SpatialReferences.Wgs84);

            // Параметри поля зору
            double heading = 0;
            double pitch = 90;
            double horizontalAngle = 90;
            double verticalAngle = 90;
            double minDistance = 1;
            double maxDistance = 50000;

            // Створення аналізу
            var viewshed = new LocationViewshed(
                 observerLocation,
                 heading,
                 pitch,
                 horizontalAngle,
                 verticalAngle,
                 minDistance,
                 maxDistance
             );

            // Призначення кольорів окремо
            //viewshed.VisibleColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            //viewshed.ObstructedColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            viewshed.IsFrustumOutlineVisible = true;


            // Створення та додавання overlay
            var analysisOverlay = new AnalysisOverlay();
            analysisOverlay.Analyses.Add(viewshed);

            // Додавання до SceneView (перевірка, чи вже додано)
            if (!view.AnalysisOverlays.Contains(analysisOverlay))
                view.AnalysisOverlays.Add(analysisOverlay);
        }

        private void AddGeodeticBufferToSceneView(SceneView view, double latitude = 48.1603, double longitude = 24.5000, double altitude = 2061)
        {
            // Точка спостереження на Говерлі
            var observerLocation = new MapPoint(longitude, latitude, altitude, SpatialReferences.Wgs84);

            // Параметри поля зору
            double heading = 0;               // напрямок (азимут)
            double pitch = 20;                // нахил вниз
            double horizontalAngle = 360;     // повна сфера по горизонталі
            double verticalAngle = 180;       // повна сфера по вертикалі
            double minDistance = 1;
            double maxDistance = 50000;       // радіус сфери

            // Створення Viewshed аналізу
            var viewshed = new LocationViewshed(
                observerLocation,
                heading,
                pitch,
                horizontalAngle,
                verticalAngle,
                minDistance,
                maxDistance
            );

            // Кольори для видимих/невидимих зон
            //viewshed.VisibleColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
            //viewshed.ObstructedColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
            viewshed.IsFrustumOutlineVisible = false;

            // Створення та додавання overlay
            var analysisOverlay = new AnalysisOverlay();
            analysisOverlay.Analyses.Add(viewshed);
            view.AnalysisOverlays.Add(analysisOverlay);
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
            if (_scene == null || TileComboBox.SelectedItem == null)
                return;

            var selectedItem = TileComboBox.SelectedItem;

            if (selectedItem is Basemap basemap)
            {
                _scene.Basemap = basemap;
            }
            else
            {
                var valueProperty = selectedItem.GetType().GetProperty("Value");
                if (valueProperty != null)
                {
                    var style = valueProperty.GetValue(selectedItem);
                    if (style is BasemapStyle basemapStyle)
                    {
                        _scene.Basemap = new Basemap(basemapStyle);
                    }
                }
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
