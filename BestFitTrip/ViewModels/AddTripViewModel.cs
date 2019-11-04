using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BestFitTrip.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BestFitTrip.ViewModels
{
    public class AddTripViewModel
    {
        [Required(ErrorMessage = "You must give a starting location")]
        public string address0 { get; set; }

        [Display(Name = "Travel Mode")]
        public ModeType Type { get; set; }
        public List<SelectListItem> ModeTypes { get; set; }

        public AddTripViewModel()
        {
            ModeTypes = new List<SelectListItem>();

            ModeTypes.Add(new SelectListItem
            {
                Value = ((int)ModeType.Driving).ToString(),
                Text = ModeType.Driving.ToString()
            });
            ModeTypes.Add(new SelectListItem
            {
                Value = ((int)ModeType.Transit).ToString(),
                Text = ModeType.Transit.ToString()
            });
            ModeTypes.Add(new SelectListItem
            {
                Value = ((int)ModeType.Walking).ToString(),
                Text = ModeType.Walking.ToString()
            });
        }

        [Required(ErrorMessage = "You must give at least 1 destination")]
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string address4 { get; set; }

        public string address5 { get; set; }

        public string address6 { get; set; }

        
    }
}
