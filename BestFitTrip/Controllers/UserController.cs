using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestFitTrip.Models;
using Microsoft.AspNetCore.Mvc;
using BestFitTrip.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestFitTrip.Controllers
{
    public class UserController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            LoginUserViewModel loginUserViewModel = new LoginUserViewModel();
            return View(loginUserViewModel);
        }

        [HttpPost]
        public IActionResult Index(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                return View("Index");
            }
            return View(loginUserViewModel);
        }

        public IActionResult Register()
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            return View(registerUserViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = RegisterUserViewModel.CreateUser(
                    registerUserViewModel.Username,
                    registerUserViewModel.Email,
                    registerUserViewModel.Password);
                //UserData.AddUser(newUser);

                ViewBag.newUserWelcome = newUser.Username;
                return View("Index");//, UserData.GetAll());
            }

            return View(registerUserViewModel);

        }
    }
}
