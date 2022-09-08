using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hospital.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }

        public string DoctorName { get; set; }

        public string DoctorSurname { get; set; }

        public string DoctorAddress { get; set; }

        public string DoctorPhoto { get; set; }

        public int DoctorDepartment { get; set; }
    }
}