using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace BestFitTrip.Models
{
    public class Trip
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ICollection<DestinationValue> Trips { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }



    }
    
}
