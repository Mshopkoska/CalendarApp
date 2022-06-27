using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CALENDAR.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CALENDAR.BusinessLogic;
using Microsoft.AspNetCore.Http;


namespace CalendarApp.Web.Controllers
{
    [Authorize]
    public class LocationsController : Controller
    {
        private readonly LocationManagement locationManagement;
        private readonly UserManager<ApplicationUser> _usermanager;

        public LocationsController(LocationManagement locationManagement, UserManager<ApplicationUser> _usermanager)
        {
            this.locationManagement = locationManagement;
            this._usermanager = _usermanager;
        }

        // GET: Location
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(locationManagement.GetUserLocations(userid));
        }

        // GET: Location/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = locationManagement.GetLocationById((int)id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            var name = form["Name"].ToString();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            locationManagement.New(name, userId);
            TempData["Alert"] = "Success! You created a location for: " + form["Name"];
            return RedirectToAction(nameof(Index));
        }
        
        /*public async Task<IActionResult> Create([Bind("Id,Name")] Location location)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    location.UserId = userId;
                     locationManagement.New(location);
                    TempData["Alert"] = "Success! You created a location for: " + location.Name;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = "An error occurred: " + ex.Message;
                    return View(location);
                }

            }
            return View(location);
        }*/

    }
}
