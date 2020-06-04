using System.ComponentModel.DataAnnotations;

namespace cw3.DAL.DTOs.Requests
{
    public class StudentDTO
    {
        [Required]
        public string IndexNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string BirthDate { get; set; }
    }
}
