using System.ComponentModel.DataAnnotations;
namespace transferguide.Models{

    public class Year1Transfer
    {
        [Required(ErrorMessage = "Transfer Course is required")]
        public required string TransferCourse { get; set; }

        [Required(ErrorMessage = "Transfer Credits are required")]
        [Range(0, 10, ErrorMessage = "Credits must be between 0 and 10")]
        public int TransferCredits { get; set; }

        [Required(ErrorMessage = "WOU Equivalent is required")]
        public required string WouEquivalent { get; set; }
    }

    public class Year2Transfer
    {
        [Required(ErrorMessage = "Transfer Course is required")]
        public required string TransferCourse { get; set; }

        [Required(ErrorMessage = "Transfer Credits are required")]
        [Range(0, 10, ErrorMessage = "Credits must be between 0 and 10")]
        public int TransferCredits { get; set; }

        [Required(ErrorMessage = "WOU Equivalent is required")]
        public required string WouEquivalent { get; set; }
    }

    public class JuniorClasses{
        [Required(ErrorMessage = "Course is required")]
        public required string Course { get; set; }

        [Required(ErrorMessage = "Credits are required")]
        [Range(0, 10, ErrorMessage = "Credits must be between 0 and 10")]
        public int Credits { get; set; }
    }

    public class SeniorClasses{
        [Required(ErrorMessage = "Course is required")]
        public required string Course { get; set; }

        [Required(ErrorMessage = "Credits are required")]
        [Range(0, 10, ErrorMessage = "Credits must be between 0 and 10")]
        public int Credits { get; set; }
    }
    public class Transfer
    {
        [Required(ErrorMessage = "College Name is required")]
        public required string CollegeName { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        public required string Degree { get; set; }
        
        [Required(ErrorMessage = "Major is required")]
        public required string Major { get; set; }

        [Required(ErrorMessage = "Advisor Name is required")]
        public required string AdvisorName { get; set; }

        [Required(ErrorMessage = "Advisor Email is required")]
        public required string AdvisorEmail { get; set; }

        [Required(ErrorMessage = "Important Notes is required")]
        public required List<string> ImportantNotes { get; set; }

         public List<Year1Transfer> Year1 { get; set; } = new List<Year1Transfer>();
         public List<Year2Transfer> Year2 { get; set; } = new List<Year2Transfer>();
        public List<JuniorClasses> Junior { get; set; } = new List<JuniorClasses>();
        public List<SeniorClasses> Senior { get; set; } = new List<SeniorClasses>();
    }
}