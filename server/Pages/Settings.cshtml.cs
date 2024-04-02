using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace server.Pages
{
    public class Settings : PageModel
    {
        private readonly ILogger<Settings> _logger;

        public Settings(ILogger<Settings> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}