using System;
using System.ComponentModel.DataAnnotations;
using BestFitTrip.Models;

namespace BestFitTrip.ViewModels
{
    public class RegisterUserViewModel
    {

        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Verify { get; set; }

        public static User CreateUser(string username, string email, string password)
        {
            User newUser = new User
            {
                Username = username,
                Email = email,
                Password = password
            };
            return newUser;
        }
    }
}
