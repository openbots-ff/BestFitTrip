using System;
using System.Collections.Generic;
using System.Linq;
using BestFitTrip.Models;
using BestFitTrip.Data;
using Microsoft.AspNetCore.Mvc;
using BestFitTrip.ViewModels;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BestFitTrip.Controllers
{
    public class UserController : Controller
    {
        private readonly TripDbContext context;
        private MD5 md5Hash = MD5.Create();

        public UserController(TripDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Login()
        {
            LoginUserViewModel loginUserViewModel = new LoginUserViewModel();
            return View(loginUserViewModel);
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel loginUserViewModel)
        {
            ViewBag.loginError = "";
            if (ModelState.IsValid)
            {
                User user = context.Users.Where(x => x.Username == loginUserViewModel.Username).SingleOrDefault();
                if (user != null && HashService.VerifyMd5Hash(md5Hash, loginUserViewModel.Password, user.Password))
                {
                    TempData["user"] = JsonConvert.SerializeObject(user);
                    return Redirect("/");
                }
                ViewBag.loginError = "Username or Password is incorrect";
            }
            return View(loginUserViewModel);
        }

        public IActionResult Logout()
        {
            TempData["user"] = null;
            return Redirect("/");
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
                if (context.Users.Where(x => x.Username == registerUserViewModel.Username).SingleOrDefault() == null)
                {
                    string hash = HashService.GetMd5Hash(md5Hash, registerUserViewModel.Password);

                    User newUser = RegisterUserViewModel.CreateUser(
                    registerUserViewModel.Username,
                    registerUserViewModel.Email,
                    hash);
                    
                    context.Users.Add(newUser);
                    context.SaveChanges();

                    TempData["user"] = JsonConvert.SerializeObject(newUser);
                    ViewBag.UserExists = "";
                    
                    return Redirect("/");
                }
                else
                {
                    ViewBag.UserExists = "This username already exists.";
                }
            }
            return View(registerUserViewModel);
        }
    }
}
