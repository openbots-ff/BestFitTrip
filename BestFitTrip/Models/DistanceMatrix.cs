using System;
using System.Collections.Generic;

namespace BestFitTrip.Models
{
    public class DistanceMatrix
    {
        public List<string> Destination_Addresses { get; set; }
        public List<string> Origin_Addresses { get; set; }
        public List<Rows> Rows { get; set; }
        public string Status { get; set; }
    }
}
