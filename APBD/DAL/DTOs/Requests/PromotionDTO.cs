using System.ComponentModel.DataAnnotations;

namespace cw3.DAL.DTOs.Requests
{
    public class PromotionDTO
    {
        [Required]
        public string Studies { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
