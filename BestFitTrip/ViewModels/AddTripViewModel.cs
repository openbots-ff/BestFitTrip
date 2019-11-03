using System;
using System.ComponentModel.DataAnnotations;
using BestFitTrip.Models;

namespace BestFitTrip.ViewModels
{
    public class AddTripViewModel
    {
        [Required]
        public string address0 { get; set; }

        [Required]
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string address4 { get; set; }

        public string address5 { get; set; }

        public string address6 { get; set; }

        //public static Trip CreateTrip(string start, string destination1,
        //    string destination2, string destination3, string destination4,
        //    string destination5, string destination6)
        //{

        //}

        public AddTripViewModel()
        {

        }
    }
}
