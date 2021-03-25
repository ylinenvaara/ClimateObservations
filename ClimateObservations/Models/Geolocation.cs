using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations
{
    public class Geolocation
    {
        public int ID { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }
        public override string ToString()
        {
            return $"{ID}, {Area}, {Country}";
        }
    }
}
