using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Enrollment
    {
        public int Semester { get; set; }

        public DateTime StartDate { get; set; }
        public Study Study { get; set; }

        public Enrollment()
        {

        }

        public static Enrollment newEnrollment(int semester, Study study, DateTime startDate)
        {
            Enrollment enrollment = new Enrollment();

            enrollment.Semester = semester;
            enrollment.StartDate = startDate;
            enrollment.Study = study;

            return enrollment;
        }


    }
}
