namespace StudentenBeheer.Models
{
    public class Docenten_modules
    {
        public int Id { get; set; }

        public Docent docent { get; set; }

        public int DocentId { get; set; }

        public Module module { get; set; }

        public int ModuleId { get; set; }



    }
}
