using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages
{
    public class LocationModel : PageModel
    {
        private readonly IGameLocationService _locationService;
        private readonly IHttpContextAccessor _httpContext;

        public GameLocationModel Location { get; private set; }
        
        
        public LocationModel(IGameLocationService locationService, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            _locationService = locationService;
        }


        public void OnGet(string location)
        {
            Location = _locationService.GetLocation(Enum.Parse<Location>(location));//parse the string into the Enum
            ViewData["LocationTitle"] = Location.Title;
            ViewData["LocationDescription"] = Location.Description;
            ViewData["BackgroundImageClass"] = Location.BackgroundImageClass;
          
        }
    }
}
