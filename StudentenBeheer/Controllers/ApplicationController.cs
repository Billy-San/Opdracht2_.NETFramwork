using Microsoft.AspNetCore.Mvc;
using StudentenBeheer.Areas.Identity.Data;
using StudentenBeheer.Data;
using StudentenBeheer.Services;

namespace StudentenBeheer.Controllers
{
    public class ApplicationController : Controller
    {
        protected readonly ApplicationUser _user;
        protected readonly ApplicationContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<ApplicationController> _logger;

        protected ApplicationController(ApplicationContext context,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<ApplicationController> logger)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            
            _user = SessionUser.GetUser(httpContextAccessor.HttpContext);
        }
    }
}


