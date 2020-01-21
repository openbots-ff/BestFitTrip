using System;
using System.ComponentModel.DataAnnotations;
using BestFitTrip.Models;

namespace BestFitTrip.ViewModels
{
    public class LoginUserViewModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
