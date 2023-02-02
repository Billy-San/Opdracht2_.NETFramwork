using Microsoft.Extensions.Localization;
using StudentenBeheer.Data;

namespace StudentenBeheer.Controllers
{
    public class Hallo_IedereenController : ApplicationController
    {
        private readonly IStringLocalizer<Hallo_IedereenController> _localizer;


        public Hallo_IedereenController(ApplicationContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger, IStringLocalizer<Hallo_IedereenController> localizer) : base(context, httpContextAccessor, logger)
        {
            _localizer = localizer;
        }

        public string Index()
        {
            return "standaard pagina om iedereen welkom te heten";
        }

        public string Welkom(string voornaam, string achternaam)
        {
            return " Welkom " + voornaam + " " + achternaam;
        }

    }
}
