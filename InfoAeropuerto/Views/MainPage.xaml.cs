using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using InfoAeropuerto.Resources;
using InfoAeropuerto.Models;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace InfoAeropuerto
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int lastPosition = 0;
        private BitmapImage flag;
        private ViewModels.InfoAeropuertoModel VM;
        private Geolocator geolocator;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            VM = (this.Resources["ViewModel"] as ViewModels.InfoAeropuertoModel);
            
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Configuracion.xaml?", UriKind.Relative));
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Config.fileName))
            {
                var items = IsolatedStorageSettings.ApplicationSettings[Config.fileName].ToString().Split('|');
                VM.Config = new Config() { searchedValue = items[0], selectedMapCartographicMode = int.Parse(items[1]), selectedMapColorMode = int.Parse(items[2]) };

                mapNokia.ColorMode = (MapColorMode)VM.Config.selectedMapColorMode;
                mapNokia.CartographicMode = (MapCartographicMode)VM.Config.selectedMapCartographicMode;

                VM.CallWebServices();
                VM.GetDownloadCompleted += vm_GetDownloadCompleted;
                
            }
        }

        void vm_GetDownloadCompleted(object sender, GenericEventArgs<bool> e)
        {
            if (e.Results)
            {
                SetFlag();
                var layer = new MapLayer();
                foreach (var aeropuerto in VM.Aeropuertos)
                {
                    var overlay = new MapOverlay();
                    overlay.GeoCoordinate = new System.Device.Location.GeoCoordinate(aeropuerto.CalcularLatitud(), aeropuerto.CalcularLongitud());
                    overlay.Content = new Image() { Source = flag, Width = 50 };
                    layer.Add(overlay);
                }
                
                mapNokia.Layers.Clear();
                mapNokia.Layers.Add(layer);
                lastPosition = 0;
                CreateLocator();
            }
        }


        void SetFlag()
        {
            flag = new BitmapImage();
            try
            {
                using (IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fileStream = ISF.OpenFile(Config.countryPic, FileMode.Open, FileAccess.Read))
                    {
                        flag.SetSource(fileStream);
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void CreateLocator()
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= geolocator_PositionChanged;
                geolocator = null;
            }

            geolocator = new Geolocator();
            geolocator.MovementThreshold = 10;
            geolocator.DesiredAccuracyInMeters = 5;
            geolocator.PositionChanged += geolocator_PositionChanged;
            await geolocator.GetGeopositionAsync();
            
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    if (mapNokia.Layers.Count > 0)
                    {

                        var meters = new Dictionary<int, double>();
                        int key = 0;
                        double distance = 0;

                        foreach (var layer in mapNokia.Layers[0])
                        {
                            GeoCoordinate airportCoordinate = new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                            distance = layer.GeoCoordinate.GetDistanceTo(airportCoordinate);
                            meters.Add(key, distance);
                            key++;
                        }

                        if (meters.Count > 0)
                        {
                            if (this.lastPosition != 0) { mapNokia.Layers[0][lastPosition].Content = new Image() { Source = flag, Width = 50 }; }
                            var closestAirport = (from entry in meters orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value).First();
                            this.lastPosition = closestAirport.Key;
                            mapNokia.Layers[0][lastPosition].Content = new Image() { Source = flag, Width = 100 };
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            });
        }
    }
}