using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations
{
    /// <summary>
    /// Climate observer
    /// </summary>
    public class Observer
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Climate observer's first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Climate observer's last name
        /// </summary>
        public string  LastName { get; set; }
        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
