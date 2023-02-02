using System.ComponentModel.DataAnnotations;

namespace StudentenBeheer.Models
{
    public class Gender
    {
        [Required]
        public char ID { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }



    }
}
