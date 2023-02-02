using Microsoft.AspNetCore.Identity;
using StudentenBeheer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentenBeheer.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    [ForeignKey("Language")]
    public string? LanguageId { get; set; }
    public Language? Language { get; set; }



}

public class ApplicationUserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Language { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Lockout { get; set; }
    public bool Student { get; set; }
    public bool Docent { get; set; }
    public bool Beheerder { get; set; }
}

