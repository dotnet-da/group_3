using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hospital.Models
{
    public class Nurse
    {
        public int NurseID { get; set; }

        public string NurseName { get; set; }

        public string NurseSurname { get; set; }

        public string NurseAddress { get; set; }

        public string NursePhoto { get; set; }

        public int NurseDoctor { get; set; }
    }
}