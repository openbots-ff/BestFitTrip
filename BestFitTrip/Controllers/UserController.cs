using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestFitTrip.Models;
using BestFitTrip.Data;
using Microsoft.AspNetCore.Mvc;
using BestFitTrip.ViewModels;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestFitTrip.Controllers
{
    public class UserController : Controller
    {
        private readonly TripDbContext context;
        private IHttpContextAccessor _contextAccessor;

        public UserController(TripDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            context = dbContext;
            _contextAccessor = contextAccessor;
        }

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
                User user = context.Users.Where(x => x.Username == loginUserViewModel.Username).SingleOrDefault();
                if (user != null && user.Password == loginUserViewModel.Password)
                {
                    //Session["username"] = user.Username;
                    //HttpContext.Session.SetString("SessionUsername", user.Username);
                    //var context = _contextAccessor.HttpContext;
                    //context.Session.SetString("SessionUsername", user.Username);
                    TempData["user"] = JsonConvert.SerializeObject(user);
                }
                return Redirect("/");
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
                if (context.Users.Where(x => x.Username == registerUserViewModel.Username).SingleOrDefault() == null)
                {
                    User newUser = RegisterUserViewModel.CreateUser(
                    registerUserViewModel.Username,
                    registerUserViewModel.Email,
                    registerUserViewModel.Password);
                    
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
