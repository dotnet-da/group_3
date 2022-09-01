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

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT DoctorID, DoctorName, DoctorSurname FROM Doctors";
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

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"INSERT INTO Doctors (DoctorName) VALUES (@DoctorName);";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@DoctorName", Doctors.DoctorName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Doctors doctors)
        {
            string query = @"UPDATE Doctors SET DoctorName = @DoctorName WHERE DoctorID = @DoctorID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@DoctorID", doctors.DoctorID);
                    myCommand.Parameters.AddWithValue("@DoctorName", doctors.DoctorName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM Doctors WHERE DoctorID = @DoctorID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@DoctorID", id);

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