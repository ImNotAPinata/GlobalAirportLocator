using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoAeropuerto.Models
{
    public class Pais
    {
        public string toponymName { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public object flag { get; set; }

        public double getlatitud()
        {
            var value = Double.Parse(this.latitud);
            return value;
        }

        public double getlongitud()
        {
            var value = Double.Parse(this.longitud);
            return value;
        }
    }
}
