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
        private static readonly HttpClient _httpClient = new HttpClient();

        public GoogleMapControl()
        {
            InitializeComponent();
            InitializeMap();
            Loaded += GoogleMapControl_Loaded;
        }

        private async void GoogleMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () => await LoadProvidersAsync());
        }

        private async Task LoadProvidersAsync()
        {
            var availableProviders = await GetAvailableProvidersAsync();

            Dispatcher.Invoke(() =>
            {
                TileComboBox.ItemsSource = availableProviders;
                TileComboBox.DisplayMemberPath = "Name";
                TileComboBox.SelectedIndex = availableProviders.Any() ? 0 : -1;
            });
        }

        private async Task<List<GMapProvider>> GetAvailableProvidersAsync()
        {
            var availableProviders = new List<GMapProvider>();

            foreach (var provider in GMapProviders.List)
            {
                if (!await IsTileServerAvailable(provider)) continue;

                availableProviders.Add(provider);
                Console.WriteLine($"✅ Доступний: {provider.Name}");
            }
            return availableProviders;
        }

        private static async Task<bool> IsTileServerAvailable(GMapProvider provider)
        {
            try
            {
                if (provider.Name.Contains("MapQuest")) return false; // Видаляємо проблемні сервери

                using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500))) // Таймаут 500 мс
                {
                    var tileTask = Task.Run(() =>
                    {
                        try
                        {
                            return provider.GetTileImage(new GPoint(512, 341), 10);
                        }
                        catch (HttpRequestException ex) when (ex.Message.Contains("401"))
                        {
                            Console.WriteLine($"❌ 401 Unauthorized: {provider.Name}");
                            return null;
                        }
                        catch (WebException ex) when (ex.Status == WebExceptionStatus.NameResolutionFailure)
                        {
                            Console.WriteLine($"❌ DNS помилка: {provider.Name}");
                            return null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Загальна помилка: {provider.Name} - {ex.Message}");
                            return null;
                        }
                    }, cts.Token);

                    var completedTask = await Task.WhenAny(tileTask, Task.Delay(500, cts.Token));
                    if (completedTask == tileTask)
                    {
                        var tileImage = await tileTask;
                        return tileImage != null;
                    }
                    else
                    {
                        Console.WriteLine($"⏳ Таймаут для {provider.Name}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Помилка доступу до {provider.Name}: {ex.Message}");
                return false;
            }
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
