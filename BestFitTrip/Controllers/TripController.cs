using System;
using System.Collections.Generic;
using System.Linq;
using BestFitTrip.Data;
using BestFitTrip.Models;
using BestFitTrip.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BestFitTrip.Controllers
{
    public class TripController : Controller
    {
        private readonly TripDbContext context;

        public TripController(TripDbContext dbContext)
        {
            context = dbContext;
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
                List<string> destinations = new List<string>() { addTripViewModel.address1 };
                if (addTripViewModel.address2 != null)
                    destinations.Add(addTripViewModel.address2);               
                if (addTripViewModel.address3 != null)
                    destinations.Add(addTripViewModel.address3);
                if (addTripViewModel.address4 != null)              
                    destinations.Add(addTripViewModel.address4);
                if (addTripViewModel.address5 != null)                
                    destinations.Add(addTripViewModel.address5);             
                if (addTripViewModel.address6 != null)
                    destinations.Add(addTripViewModel.address6);
                
                string mode = addTripViewModel.Type.ToString().ToLower();
                List<DestinationValue> destinationValues = DestinationValue.GetDistancesOrdered(origin, destinations, mode);
                ViewBag.orderedTrips = destinationValues;

                TempData["destinationValues"] = JsonConvert.SerializeObject(destinationValues);//destinationValues;
                TempData["mode"] = mode;

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

                object u;
                TempData.TryGetValue("user", out u);
                User user = JsonConvert.DeserializeObject<User>((string)u);

                Trip newTrip = new Trip
                {
                    DestinationValues = destinationValues,
                    Title = title,
                    Mode = TempData["mode"].ToString(),
                    UserID = user.ID
                };

                context.Trips.Add(newTrip);
                context.SaveChanges();

                return Redirect("/Trip/MyTrips");
            }
            return Redirect("/User/Login");
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
                ViewBag.Username = user.Username;
                TempData.Keep(); //dont delete
                return View();
            }
            return Redirect("/User/Login");
        }

        [HttpPost]
        public IActionResult Delete(int tripID)
        {
            Trip trip = context.Trips.Single(t => t.ID == tripID);
            context.Trips.Remove(trip);
            context.SaveChanges();

            return Redirect("/Trip/MyTrips");
        }

        [HttpGet]
        public IActionResult GetDirections(int tripID)
        {
            if (TempData["user"] != null)
            {
                object u;
                TempData.TryGetValue("user", out u);
                User user = JsonConvert.DeserializeObject<User>((string)u);
                user = context.Users.Include("Trips").Where(x => x.ID == user.ID).SingleOrDefault();

                if (user != null && ContainsTrip(user, tripID))
                {
                    Trip trip = context.Trips.Include("DestinationValues").Single(t => t.ID == tripID);
                    ViewBag.MyTrip = trip;

                    return View();
                }
            } 
            return Redirect("/");
        }

        public bool ContainsTrip(User user, int tripID)
        {
            foreach(Trip trip in user.Trips)
            {
                if(trip.ID == tripID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
