using StudentenBeheer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentenBeheer.Models
{
    public class Docent
    {

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }


        [ForeignKey("user_docent")]
        public string? UserId { get; set; }
        public ApplicationUser? user_docent { get; set; }


        [ForeignKey("Gender_docent")]
        public char GenderId { get; set; }

        public Gender? Gender_docent { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedAt { get; set; } = DateTime.MaxValue;



    }
}
