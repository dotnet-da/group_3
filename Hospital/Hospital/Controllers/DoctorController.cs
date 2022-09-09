using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        // dependency injection to read connection strings

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public DoctorController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // get all doctors

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"SELECT ID, Name, Surname, Department_Name AS Department
                            FROM Doctor INNER JOIN Department ON Doctor.Department_ID = Department.Department_ID";

            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        // get one doctor

        [HttpGet("{id}")]
        public JsonResult GetOne(int id)
        {
            string query = @"SELECT ID, Name, Surname, Department_Name AS Department
                            FROM Doctor INNER JOIN Department ON Doctor.Department_ID = Department.Department_ID
                            WHERE ID = @ID";

            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        // get doctors that have nurses assigned to them

        [HttpGet("nurse_assigned/")]
        public JsonResult GetWithAssignedNurses()
        {
            string query = @"SELECT Doctor.ID, Doctor.Name, Doctor.Surname, Doctor.Department_ID
                            FROM Doctor RIGHT JOIN Nurse ON Doctor.ID = Nurse.Doctor_ID";

            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        // get doctors that do not have nurses assigned to them

        [HttpGet("nurse_unassigned/")]
        public JsonResult GetWithUnassignedNurses()
        {
            string query = @"SELECT Doctor.ID, Doctor.Name, Doctor.Surname, Doctor.Department_ID
                            FROM Doctor LEFT JOIN Nurse ON Doctor.ID = Nurse.Doctor_ID
                            WHERE Nurse.Doctor_ID IS NULL";

            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        // add a new doctor

        [HttpPost]
        public JsonResult Post([FromBody] Doctor doctor)
        {
            string query = @"INSERT INTO Doctor (Name, Surname, Address, Department_ID) VALUES (@Name, @Surname, @Address, @Department_ID);";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Name", doctor.DoctorName);
                    myCommand.Parameters.AddWithValue("@Surname", doctor.DoctorSurname);
                    myCommand.Parameters.AddWithValue("@Address", doctor.DoctorAddress);
                    myCommand.Parameters.AddWithValue("@Department_ID", doctor.DoctorDepartment);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        // update a doctor 

        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody] Doctor doctor)
        {
            string query = @"UPDATE Doctor SET Name = @Name, Surname = @Surname, Address = @Address, Department_ID = @Department_ID WHERE ID = @ID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);
                    myCommand.Parameters.AddWithValue("@Name", doctor.DoctorName);
                    myCommand.Parameters.AddWithValue("@Surname", doctor.DoctorSurname);
                    myCommand.Parameters.AddWithValue("@Address", doctor.DoctorAddress);
                    myCommand.Parameters.AddWithValue("@Department_ID", doctor.DoctorDepartment);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        // delete a doctor

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM Doctor WHERE ID = @ID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        // upload a doctor image

        [Route("save_file")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("default.png");
            }
        }
    }
}