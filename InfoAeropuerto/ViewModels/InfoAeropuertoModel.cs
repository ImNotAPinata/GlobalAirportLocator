using InfoAeropuerto.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace InfoAeropuerto.ViewModels
{
    public class InfoAeropuertoModel : ViewModelBase
    {
        Config config;
        public Config Config
        {
            get { return config; }
            set { config = value; OnPropertyChanged(); }
        }

        IEnumerable<Aeropuerto> aeropuertos;
        public IEnumerable<Aeropuerto> Aeropuertos
        {
            get { return aeropuertos; }
            set { aeropuertos = value; OnPropertyChanged(); }
        }

        Pais pais;
        public Pais Pais
        {
            get { return pais; }
            set { pais = value; OnPropertyChanged(); }
        }

        /*
        public IEnumerable<Microsoft.Phone.Maps.Controls.MapCartographicMode> MapCartographicMode
        {
            get
            {
                return Enum.GetValues(typeof(Microsoft.Phone.Maps.Controls.MapCartographicMode)).Cast<Microsoft.Phone.Maps.Controls.MapCartographicMode>();
            }
        }
        public IEnumerable<Microsoft.Phone.Maps.Controls.MapColorMode> MapColorMode
        {
            get
            {
                return Enum.GetValues(typeof(Microsoft.Phone.Maps.Controls.MapColorMode)).Cast<Microsoft.Phone.Maps.Controls.MapColorMode>();
            }
        }
        */

        ServiceModel serviceModel = new ServiceModel();
        public event EventHandler<GenericEventArgs<bool>> GetDownloadCompleted;

        public InfoAeropuertoModel()
        {
            serviceModel.GetAeropuertoCompleted += (s, a) => {
                Aeropuertos = new List<Aeropuerto>(a.Results);
                if (GetDownloadCompleted != null)
                {
                    GetDownloadCompleted(this, new GenericEventArgs<bool>(true));
                }
            };
        }

        public void CallWebServices()
        {
            serviceModel.getPaisInfo(this.config.searchedValue);
        }
    }
}
