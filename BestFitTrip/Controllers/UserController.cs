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
using System.Security.Cryptography;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestFitTrip.Controllers
{
    public class UserController : Controller
    {
        private readonly TripDbContext context;
        private IHttpContextAccessor _contextAccessor;
        private MD5 md5Hash = MD5.Create();

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
                //string hash = HashService.GetMd5Hash(md5Hash, user.Password);
                if (user != null && HashService.VerifyMd5Hash(md5Hash, loginUserViewModel.Password, user.Password))
                {
                    //Session["username"] = user.Username;
                    //HttpContext.Session.SetString("SessionUsername", user.Username);
                    //var context = _contextAccessor.HttpContext;
                    //context.Session.SetString("SessionUsername", user.Username);
                    TempData["user"] = JsonConvert.SerializeObject(user);
                    //TempData.Keep();
                }
                return Redirect("/");
            }
            return View(loginUserViewModel);
        }

        public IActionResult Logout()
        {
            TempData["user"] = null;
            return Redirect("/User");
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
