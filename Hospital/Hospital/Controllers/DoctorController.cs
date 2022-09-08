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

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        // dependency injection to read connection strings

        private readonly IConfiguration _configuration;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
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

        // get doctors in a department

        [HttpGet("{department_id}")]
        public JsonResult GetByDepartment(int id)
        {
            string query = @"SELECT ID, Name, Surname, Department_Name FROM Doctor INNER JOIN Department ON Doctor.Department_ID = Department.Department_ID WHERE Department_ID = @Department_ID";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Department_ID", id);

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
        public JsonResult Post(Doctor doctor)
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

        [HttpPut]
        public JsonResult Put(Doctor doctor)
        {
            string query = @"UPDATE Doctor SET Name = @Name, Surname = @Surname, Address = @Address, @Department_ID = @Department_ID WHERE ID = @ID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@ID", doctor.DoctorID);
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

    }
}