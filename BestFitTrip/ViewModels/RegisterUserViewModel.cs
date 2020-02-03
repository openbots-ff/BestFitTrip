using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BestFitTrip.Models;

namespace BestFitTrip.ViewModels
{
    public class RegisterUserViewModel
    {

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Username { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [DisplayName("Verify Password")]
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
