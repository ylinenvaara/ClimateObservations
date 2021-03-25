using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations
{
    public class Measurement
    {
        public int ID { get; set; }
        public double? Value { get; set; }
        public string Unit { get; set; }
        public int CategoryID { get; set; }
        public string Category{ get; set; }
        public string BaseCategory { get; set; }
        public int ObservationID { get; set; }
        public override string ToString()
        {
            if (Value == null && BaseCategory == null)
            {
                return $"{Category}";
            }
            else if (Value == null)
            {
                return $"{BaseCategory}, {Category}";
            }
            else if (BaseCategory == null)
            {
                return $"{Value} {Unit} {Category}";
            }
            else
            {
                return $"{Value} {Unit} {BaseCategory}, {Category}";
            }
        }
    }
}
