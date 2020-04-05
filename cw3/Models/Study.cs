using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Study
    {
        public string Name { get; set;}

        public Study()
        {

        }

        public static Study newStudy(string name)
        {
            Study study = new Study();

            study.Name = name;
           
            return study;
        }
    }
}
