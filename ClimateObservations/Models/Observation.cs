using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations
{
    public class Observation
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int ObserverID { get; set; }
        public int GeolocationID { get; set; }
        public override string ToString()
        {
            return $"{Date} på geopunkt {GeolocationID}";
        }
    }
}
