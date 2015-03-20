using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Xml.Linq;

namespace InfoAeropuerto.Models
{
    public class ServiceModel
    {
        public event EventHandler<GenericListEventArgs<Aeropuerto>> GetAeropuertoCompleted;
        //public event EventHandler<GenericEventArgs<Pais>> GetPaisCompleted;
        
        public void getPaisInfo(string value)
        {
            try
            {
                string uri = string.Format("http://api.geonames.org/search?q={0}&maxRows=1&username=jerrysb",value);
                                
                WebClient client = new WebClient();
                client.DownloadStringCompleted += (s, a) =>
                {
                    if (a.Error == null && !a.Cancelled)
                    {
                        var result = a.Result;
                        var doc = XDocument.Parse(result);
                        var XML = doc.Descendants("geonames").SingleOrDefault().Descendants("geoname").Single();
                        var pais = new Pais()
                        {
                            countryCode = XML.Element("countryCode").Value,
                            toponymName = XML.Element("toponymName").Value,
                            countryName = XML.Element("countryName").Value,
                            latitud = XML.Element("lat").Value,
                            longitud = XML.Element("lng").Value,
                            name = XML.Element("name").Value,
                        };

                        getCountryAirports(pais.countryName);
                        getCountryFlag(pais.countryCode);
                        
                    }
                };
                client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));                
            }
            catch (Exception e)
            {
                MessageBox.Show("Se encontro un error! " + e.Message);
            }
        }

        private bool EsRepetido(List<Aeropuerto> lista, Aeropuerto ap)
        {
            foreach (var item in lista)
            {
                // **MEGA** IF ROFL!
                if (item.LatitudeDegree == ap.LatitudeDegree &&
                    item.LatitudeMinute == ap.LatitudeMinute &&
                    item.LatitudeNpeerS == ap.LatitudeNpeerS &&
                    item.LatitudeSecond == ap.LatitudeSecond &&
                    item.LongitudeDegree == ap.LongitudeDegree &&
                    item.LongitudeEperW == ap.LongitudeEperW &&
                    item.LongitudeMinute == ap.LongitudeMinute &&
                    item.LongitudeSeconds == ap.LongitudeSeconds
                    )
                    return true;
            }
            return false;
        }

        void getCountryAirports(string countryName) 
        {
            string uriX = string.Format("http://www.webservicex.net/airport.asmx/GetAirportInformationByCountry?country={0}", countryName);

            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (x, b) =>
            {
                if (b.Error == null && !b.Cancelled)
                {
                    var docX = XDocument.Parse(b.Result);
                    var XMLX = XDocument.Parse(docX.Document.Root.Value);
                    var list = new List<Aeropuerto>();
                    foreach (XElement c in XMLX.Descendants("Table").ToList())
                    {
                        var ap = new Aeropuerto();
                        ap.AirportCode = c.Element("AirportCode").Value;
                        ap.CityOrAirportName = c.Element("CityOrAirportName").Value;
                        ap.Country = c.Element("Country").Value;
                        ap.GMTOffset = c.Element("GMTOffset") == null ? "0" : c.Element("GMTOffset").Value;
                        ap.LatitudeDegree = c.Element("LatitudeDegree").Value;
                        ap.LatitudeMinute = c.Element("LatitudeMinute").Value;
                        ap.LatitudeNpeerS = c.Element("LatitudeNpeerS").Value;
                        ap.LatitudeSecond = c.Element("LatitudeSecond").Value;
                        ap.LongitudeDegree = c.Element("LongitudeDegree").Value;
                        ap.LongitudeEperW = c.Element("LongitudeEperW").Value;
                        ap.LongitudeMinute = c.Element("LongitudeMinute").Value;
                        ap.LongitudeSeconds = c.Element("LongitudeSeconds").Value;
                        ap.RunwayElevationFeet = c.Element("RunwayElevationFeet").Value;
                        ap.RunwayLengthFeet = c.Element("RunwayLengthFeet").Value;

                        // Si no es repetido agregalo!
                        if (!EsRepetido(list, ap))
                        {
                            list.Add(ap);
                        }
                    }

                    if (GetAeropuertoCompleted != null)
                    {
                        GetAeropuertoCompleted(this, new GenericListEventArgs<Aeropuerto>(list));
                    }

                }
            };
            wc.DownloadStringAsync(new Uri(uriX, UriKind.Absolute));
        }

        void getCountryFlag(string countryCode)
        {
            var url = string.Format("http://www.geonames.org/flags/x/{0}.gif", countryCode.ToLower());
            WebClient client = new WebClient();
            client.OpenReadCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    using (IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (ISF.FileExists(Config.countryPic))
                            ISF.DeleteFile(Config.countryPic);

                        IsolatedStorageFileStream fileStream = ISF.CreateFile(Config.countryPic);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(a.Result);

                        WriteableBitmap wb = new WriteableBitmap(bitmap);
                        System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                        fileStream.Close();
                    }
                }
            };
            client.OpenReadAsync(new Uri(url, UriKind.Absolute));
        }
    }
}
