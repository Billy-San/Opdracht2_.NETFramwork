using StudentenBeheer.Areas.Identity.Data;
using StudentenBeheer.Data;

namespace StudentenBeheer.Services
{
    public class SessionUser
    {
        class UserStats
        {
            public DateTime LastEntered { get; set; }
            public int Count { get; set; }
            public ApplicationUser? User { get; set; }
        }


        readonly RequestDelegate _next;
        static Dictionary<string, UserStats> SessionUserDictionary = new Dictionary<string, UserStats>();

        public SessionUser(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ApplicationContext dbContext)
        {
            string SessionUser_name = httpContext.User.Identity.Name == null ? "-" : httpContext.User.Identity.Name;
            try
            {
                UserStats us = SessionUserDictionary[SessionUser_name];
                us.Count++;
                us.LastEntered = DateTime.Now;
            }
            catch
            {
                SessionUserDictionary[SessionUser_name] = new UserStats
                {
                    User = dbContext.Users.FirstOrDefault(u => u.UserName == SessionUser_name),
                    Count = 1,
                    LastEntered = DateTime.Now
                };
            }

            await _next(httpContext);
        }

        public static ApplicationUser GetUser(HttpContext httpContext)
        {
            return SessionUserDictionary[httpContext.User.Identity.Name == null ? "-" : httpContext.User.Identity.Name].User;
        }
    }
}
