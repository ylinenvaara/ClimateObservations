using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? BasecategoryID { get; set; }
        public string Unit { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
