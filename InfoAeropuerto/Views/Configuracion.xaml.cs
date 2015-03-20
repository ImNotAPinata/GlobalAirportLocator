using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using InfoAeropuerto.Models;
using System.IO;

namespace InfoAeropuerto
{
    public partial class Configuracion : PhoneApplicationPage
    {
        public Configuracion()
        {
            InitializeComponent();
            this.lbxCarthographicMapa.ItemsSource = Enum.GetValues(typeof(Microsoft.Phone.Maps.Controls.MapCartographicMode));
            this.lbxCarthographicMapa.SelectedIndex = 0;
            this.lbxColorMapa.ItemsSource = Enum.GetValues(typeof(Microsoft.Phone.Maps.Controls.MapColorMode));
            this.lbxColorMapa.SelectedIndex = 0;
            this.Loaded += Configuracion_Loaded;
        }

        void Configuracion_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Config.fileName))
            {
                var items = IsolatedStorageSettings.ApplicationSettings[Config.fileName].ToString().Split('|');
                txtpais.Text = items[0];
                lbxCarthographicMapa.SelectedIndex = int.Parse(items[1]);
                lbxColorMapa.SelectedIndex = int.Parse(items[2]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Config.fileName))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(Config.fileName);
            }

            /*Descargo y Guardo*/

            var configvalues = string.Format("{0}|{1}|{2}", txtpais.Text, lbxCarthographicMapa.SelectedIndex, lbxColorMapa.SelectedIndex);
            IsolatedStorageSettings.ApplicationSettings.Add(Config.fileName, configvalues);
            
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml?", UriKind.Relative));
        }



    }
}