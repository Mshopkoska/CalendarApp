using CalendarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CALENDAR.Data;
using CALENDAR.Entity;
using CALENDAR.Helpers;
using CALENDAR.BusinessLogic;

namespace CalendarApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventManagement eventManagment;
        private readonly LocationManagement locationManagement;
        private readonly UserManager<ApplicationUser> _usermanager;

        public HomeController(EventManagement eventManagment, LocationManagement locationManagement, ILogger<HomeController> _logger, IDAL _idal, UserManager<ApplicationUser> _usermanager)
        {
            this._logger = _logger;
            this.eventManagment = eventManagment;
            this.locationManagement = locationManagement;
            this._usermanager = _usermanager;
        }

        public IActionResult Index()
        {
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(locationManagement.GetLocations());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(eventManagment.GetEvents());
            return View();
        }

        [Authorize]
        public IActionResult MyCalendar()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(locationManagement.GetUserLocations(userid));
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(eventManagment.GetUserEvents(userid));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
