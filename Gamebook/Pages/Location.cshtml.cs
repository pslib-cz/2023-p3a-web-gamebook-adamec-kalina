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

        public GameLocationModel Location { get; private set; }



        public LocationModel(IGameLocationService locationService)
        {
            _locationService = locationService;
        }



        public void OnGet(Location location)
        {
            Location = _locationService.GetLocation(location);
        }
    }
}
