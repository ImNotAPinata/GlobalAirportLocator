using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoAeropuerto.Models
{
    public class Config
    {
        public const string fileName = "config.txt";
        public const string countryPic = "pic.jpg";

        public string searchedValue {get;set;}
        public object selectedMapCartographicMode { get; set; }
        public object selectedMapColorMode { get; set; }
        
    }
}
