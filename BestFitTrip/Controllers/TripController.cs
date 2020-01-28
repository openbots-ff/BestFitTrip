using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestFitTrip.Data;
using BestFitTrip.Models;
using BestFitTrip.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestFitTrip.Controllers
{
    public class TripController : Controller
    {
        //public List<DestinationValue> destinationValues = new List<DestinationValue>();
        // GET: /<controller>/
        private readonly TripDbContext context;
        private IHttpContextAccessor _contextAccessor;

        public TripController(TripDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            context = dbContext;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            AddTripViewModel addTripViewModel = new AddTripViewModel();
            return View(addTripViewModel);
        }

        [HttpPost]
        public IActionResult Index(AddTripViewModel addTripViewModel)
        {
            if (ModelState.IsValid)
            {
                string origin = addTripViewModel.address0;
                List<string> destinations = new List<string>();
                destinations.Add(addTripViewModel.address1);
                if (addTripViewModel.address2 != null)
                {
                    destinations.Add(addTripViewModel.address2);
                }
                if (addTripViewModel.address3 != null)
                {
                    destinations.Add(addTripViewModel.address3);
                }
                if (addTripViewModel.address4 != null)
                {
                    destinations.Add(addTripViewModel.address4);
                }
                if (addTripViewModel.address5 != null)
                {
                    destinations.Add(addTripViewModel.address5);
                }
                if (addTripViewModel.address6 != null)
                {
                    destinations.Add(addTripViewModel.address6);
                }
                
                string mode = addTripViewModel.Type.ToString().ToLower();
                List<DestinationValue> destinationValues = DestinationValue.GetDistancesOrdered(origin, destinations, mode);
                ViewBag.orderedTrips = destinationValues;

                TempData["destinationValues"] = JsonConvert.SerializeObject(destinationValues);//destinationValues;

                return View(addTripViewModel);
            }
            return View(addTripViewModel);
        }

        [HttpPost]
        public IActionResult SaveTrip(string title)
        {
            if (TempData["user"] != null)
            {
                object d;
                TempData.TryGetValue("destinationValues", out d);
                List<DestinationValue> destinationValues = JsonConvert.DeserializeObject<List<DestinationValue>>((string)d);
                //var username = HttpContext.Session.GetString("SessionUsername");
                //var context = _contextAccessor.HttpContext;
                //var username = context.Session.GetString("SessionUsername");
                object u;
                TempData.TryGetValue("user", out u);
                User user = JsonConvert.DeserializeObject<User>((string)u);

                Trip newTrip = new Trip
                {
                    DestinationValues = destinationValues,
                    Title = title,
                    //User = user,
                    UserID = user.ID
                };

                context.Trips.Add(newTrip);
                context.SaveChanges();

                return Redirect("/Trip/MyTrips");
            }
            else
            {
                return Redirect("/User");
            }
        }

        [HttpGet]
        public IActionResult MyTrips()
        {
            if (TempData["user"] != null)
            {
                object u;
                TempData.TryGetValue("user", out u);
                User user = JsonConvert.DeserializeObject<User>((string)u);
                List<Trip> myTrips = context.Trips.Include("DestinationValues").Where(x => x.UserID == user.ID).OrderBy(x => x.Title).ToList();
                ViewBag.MyTrips = myTrips;
                TempData.Keep();
                return View();
            }
            else
            {
                return Redirect("/User");
            }
        }

        [HttpPost]
        public IActionResult Delete(int tripID)
        {
            Trip trip = context.Trips.Single(t => t.ID == tripID);
            context.Trips.Remove(trip);
            context.SaveChanges();

            return Redirect("/Trip/MyTrips");
        }
    }
}
