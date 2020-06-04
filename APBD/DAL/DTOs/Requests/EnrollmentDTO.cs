﻿using System.ComponentModel.DataAnnotations;

namespace cw3.DAL.DTO
{
    public class EnrollmentDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string IndexNumber { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public string Studies { get; set; }

    }
}


