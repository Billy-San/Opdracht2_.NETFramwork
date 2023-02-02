using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentenBeheer.Areas.Identity.Data;
using StudentenBeheer.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class UsersController : ApplicationController
    {
        public UsersController(ApplicationContext context,
                                 IHttpContextAccessor httpContextAccessor,
                                 ILogger<ApplicationController> logger)
             : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index(string userName, string name, string email, int? pageNumber)
        {
            if (userName == null) userName = "";
            if (name == null) name = "";
            if (email == null) email = "";
            List<ApplicationUser> users =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (name == "" || (u.Firstname.Contains(name) || u.Lastname.Contains(name)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.Lastname + " " + u.Firstname)
                .ToList();
            List<ApplicationUserViewModel> userViewModels = new List<ApplicationUserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.Firstname,
                    Language = Language.LanguageDictionary[user.LanguageId].Name,
                    LastName = user.Lastname,
                    Lockout = user.LockoutEnd != null,
                    PhoneNumber = user.PhoneNumber,
                    Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                    Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                    Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = name;
            ViewData["email"] = email;
            if (pageNumber == null) pageNumber = 1;
            PaginatedList<ApplicationUserViewModel> model = new PaginatedList<ApplicationUserViewModel>(userViewModels, userViewModels.Count, 1, 10);
            return View(model);
        }

        public async Task<ActionResult> Locking(string id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user.LockoutEnd != null)
                user.LockoutEnd = null;
            else
                user.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Roles(string id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            ApplicationUserViewModel model = new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.Firstname,
                Language = Language.LanguageDictionary[user.LanguageId].Name,
                LastName = user.Lastname,
                Lockout = user.LockoutEnd != null,
                PhoneNumber = user.PhoneNumber,
                Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles([Bind("Id, UserName, FirstName, LastName, Student, Docent, Beheerder")] ApplicationUserViewModel model)
        {
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == model.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }
            if (model.Student) _context.Add(new IdentityUserRole<string> { RoleId = "Student", UserId = model.Id });
            if (model.Docent) _context.Add(new IdentityUserRole<string> { RoleId = "Docent", UserId = model.Id });
            if (model.Beheerder) _context.Add(new IdentityUserRole<string> { RoleId = "Beheerder", UserId = model.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }
}
