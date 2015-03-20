using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoAeropuerto.Models
{
    public class Aeropuerto
    {
        public string AirportCode { get; set; }
        public string CityOrAirportName { get; set; }
        public string Country { get; set; }
        public string GMTOffset { get; set; }
        public string RunwayLengthFeet { get; set; }
        public string RunwayElevationFeet { get; set; }
        public string LatitudeDegree { get; set; }
        public string LatitudeMinute { get; set; }
        public string LatitudeSecond { get; set; }
        public string LatitudeNpeerS { get; set; }
        public string LongitudeDegree { get; set; }
        public string LongitudeMinute { get; set; }
        public string LongitudeSeconds { get; set; }
        public string LongitudeEperW { get; set; }

        public double CalcularLatitud()
        {
            if (this.LatitudeDegree != null && this.LatitudeMinute != null && this.LatitudeSecond != null)
            {
                var DegreeLat = Double.Parse(this.LatitudeDegree);
                var MinuteLat = Double.Parse(this.LatitudeMinute);
                var SecondLat = Double.Parse(this.LatitudeSecond);

                var valueLat = Math.Round(DegreeLat + (MinuteLat / 60) + (SecondLat / 3600), 5);
                if (this.LatitudeNpeerS == "S") { return -System.Math.Abs(valueLat); }
                return valueLat;
            }
            else return 0;
        }

        public double CalcularLongitud()
        {
            if (this.LongitudeDegree != null && this.LongitudeMinute != null && this.LongitudeSeconds != null)
            {
                var Degree = Double.Parse(this.LongitudeDegree);
                var Minute = Double.Parse(this.LongitudeMinute);
                var Second = Double.Parse(this.LongitudeSeconds);

                var value = Math.Round(Degree + (Minute / 60) + (Second / 3600), 5);
                if (this.LongitudeEperW == "W") { value = -System.Math.Abs(value); }
                return value;
            }
            else return 0;
        }
    }
}
