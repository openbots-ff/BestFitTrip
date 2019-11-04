using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestFitTrip.Models;
using BestFitTrip.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestFitTrip.Controllers
{
    public class TripController : Controller
    {
        // GET: /<controller>/
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
                ViewBag.orderedTrips = DestinationValue.GetDistancesOrdered(origin, destinations, mode);
                

                return View(addTripViewModel);
            }
            return View(addTripViewModel);
        }
    }
}
