using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentenBeheer.Areas.Identity.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Data
{

    public static class SeedDatabase
    {

        public static void Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Language.Any())
                {
                    context.Language.AddRange(
                        new Language() { Id = "-", Name = "-", Cultures = "-", IsSystemLanguage = false },
                        new Language() { Id = "de", Name = "Deutsch", Cultures = "DE", IsSystemLanguage = false },
                        new Language() { Id = "en", Name = "English", Cultures = "UK;US", IsSystemLanguage = true },
                        new Language() { Id = "es", Name = "Español", Cultures = "ES", IsSystemLanguage = false },
                        new Language() { Id = "fr", Name = "français", Cultures = "BE;FR", IsSystemLanguage = true },
                        new Language() { Id = "nl", Name = "Nederlands", Cultures = "BE;NL", IsSystemLanguage = true }
                    );
                    context.SaveChanges();
                }



                ApplicationUser Beheerder = null;
                ApplicationUser Docent = null;
                ApplicationUser Student1 = null;


                if (!context.Users.Any())
                {
                    ApplicationUser dummy = new ApplicationUser { Id = "-", Firstname = "-", Lastname = "-", UserName = "-", Email = "?@?.?", LanguageId = "-" };
                    context.Users.Add(dummy);
                    context.SaveChanges();

                    Beheerder = new ApplicationUser
                    {
                        UserName = "Beheerder1",
                        Firstname = "Bilal",
                        Lastname = "Bakkali",
                        Email = "System.administrator@studentenbeheer.be",
                        LanguageId = "nl",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(Beheerder, "Aaaa");
                    Thread.Sleep(3000);

                    Docent = new ApplicationUser
                    {
                        UserName = "Docent1",
                        Firstname = "David",
                        Lastname = "dav",
                        Email = "System.User@studentenbeheer.be",
                        LanguageId = "nl",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(Docent, "Aaaa");
                    Thread.Sleep(3000);

                    Student1 = new ApplicationUser
                    {
                        UserName = "Student1",
                        Firstname = "testt",
                        Lastname = "Docent2",
                        Email = "System.User@studentenbeheer.be",
                        LanguageId = "nl",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(Student1, "Aaaa");
                    Thread.Sleep(3000);





                }



                if (!context.Roles.Any())
                {

                    context.Roles.AddRange(

                            new IdentityRole { Id = "Beheerder", Name = "Beheerder", NormalizedName = "beheerder" },
                            new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "docent" },
                            new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "student" }

                            );

                    context.SaveChanges();
                }


                if (!context.Gender.Any() || !(context.Student.Any()))
                {
                    
                    context.Gender.AddRange(

                       new Gender
                       {

                           ID = 'M',
                           Name = "Male"


                       },

                       new Gender
                       {

                           ID = 'F',
                           Name = "Female"


                       },

                       new Gender
                       {
                           ID = 'X',
                           Name = "Not set"
                       }

                   );
                    context.SaveChanges();

                    context.Student.AddRange(

                               new Student
                               {
                                   Name = "Bilal",
                                   Lastname = "Bakkali",
                                   Birthday = DateTime.Now,
                                   GenderId = 'M',
                                   UserId = Student1.Id,
                                   Deleted = DateTime.MaxValue


                               },
                               new Student
                               {
                                   Name = "Bilal2",
                                   Lastname = "Bakkali2",
                                   Birthday = DateTime.Now,
                                   GenderId = 'F',
                                   Deleted = DateTime.Now


                               },
                                 new Student
                                 {
                                     Name = "Mark",
                                     Lastname = "maribelle",
                                     Birthday = DateTime.Now,
                                     GenderId = 'F',
                                     Deleted = DateTime.Now


                                 },
                                   new Student
                                   {
                                       Name = "insaf",
                                       Lastname = "ben",
                                       Birthday = DateTime.Now,
                                       GenderId = 'M',
                                       Deleted = DateTime.Now


                                   }
                        );
                    context.SaveChanges();

                }

                if (!context.Docent.Any())
                {
                    context.Docent.AddRange(

                          new Docent
                          {
                              FirstName = "Docent1",
                              LastName = "Docent1",
                              Birthday = DateTime.Now,
                              GenderId = 'F',
                              UserId = Docent.Id,
                              Email = "Docent1@docent.be",
                              DeletedAt = DateTime.MaxValue
                          }

                        );
                    context.SaveChanges();
                }



                if (!context.Module.Any())
                {
                    context.Module.AddRange(

                    new Module
                    {
                        Name = "Backend web",
                        Omschrijving = "Laravel,Php",
                        Deleted = DateTime.MaxValue
                    },
                     new Module
                     {
                         Name = "Dynamic web",
                         Omschrijving = "Javascript",
                         Deleted = DateTime.Now
                     },
                      new Module
                      {
                          Name = "Java Fundamentals",
                          Omschrijving = "Basis principe java",
                          Deleted = DateTime.MaxValue
                      }

                    );

                    context.SaveChanges();

                }
                if (!context.Docenten_modules.Any())
                {
                    context.Docenten_modules.AddRange(

                        new Docenten_modules
                        {
                            ModuleId = 1,
                            DocentId = 1

                        }

                        );
                }



                if (Beheerder != null)
                {
                    context.UserRoles.AddRange(

                        new IdentityUserRole<string> { UserId = Beheerder.Id, RoleId = "Beheerder" },
                        new IdentityUserRole<string> { UserId = Docent.Id, RoleId = "Docent" },
                        new IdentityUserRole<string> { UserId = Student1.Id, RoleId = "Student" }

                        );


                    context.SaveChanges();
                }






                List<string> supportedLanguages = new List<string>();
                Language.AllLanguages = context.Language.ToList();
                Language.LanguageDictionary = new Dictionary<string, Language>();
                Language.SystemLanguages = new List<Language>();

                supportedLanguages.Add("nl-BE");
                foreach (Language l in Language.AllLanguages)
                {

                    Language.LanguageDictionary[l.Id] = l;


                    if (l.Id != "-")
                    {

                        if (l.IsSystemLanguage)
                            Language.SystemLanguages.Add(l);
                        supportedLanguages.Add(l.Id);
                        string[] even = l.Cultures.Split(";");
                        foreach (string e in even)
                        {
                            supportedLanguages.Add(l.Id + "-" + e);
                        }
                    }
                }
                Language.SupportedLanguages = supportedLanguages.ToArray();

            }

        }
    }

}

