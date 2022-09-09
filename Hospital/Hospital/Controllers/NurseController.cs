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
    public class NurseController : ControllerBase
    {
        // dependency injection to read connection strings

        private readonly IConfiguration _configuration;
        public NurseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // get all nurses

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"SELECT Nurse.ID, Nurse.Name, Nurse.Surname, Nurse.Doctor_ID AS Doctor, Department_ID AS Department
                            FROM Nurse INNER JOIN Doctor ON Nurse.Doctor_ID = Doctor.ID";

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

        // get one nurse

        [HttpGet("{id}")]
        public JsonResult GetOne(int id)
        {
            string query = @"SELECT Nurse.ID, Nurse.Name, Nurse.Surname, Nurse.Doctor_ID AS Doctor, Department_ID AS Department
                            FROM Nurse INNER JOIN Doctor ON Nurse.Doctor_ID = Doctor.ID
                            WHERE Nurse.ID = @ID";

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

        // add a new nurse

        [HttpPost]
        public JsonResult Post([FromBody] Nurse nurse)
        {
            string query = @"INSERT INTO Nurse (Name, Surname, Address, Doctor_ID) VALUES (@Name, @Surname, @Address, @Doctor_ID);";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Name", nurse.NurseName);
                    myCommand.Parameters.AddWithValue("@Surname", nurse.NurseSurname);
                    myCommand.Parameters.AddWithValue("@Address", nurse.NurseAddress);
                    myCommand.Parameters.AddWithValue("@Doctor_ID", nurse.NurseDoctor);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        // update a nurse

        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody] Nurse nurse)
        {
            string query = @"UPDATE Nurse SET Name = @Name, Surname = @Surname, Address = @Address, Doctor_ID = @Doctor_ID WHERE ID = @ID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);
                    myCommand.Parameters.AddWithValue("@Name", nurse.NurseName);
                    myCommand.Parameters.AddWithValue("@Surname", nurse.NurseSurname);
                    myCommand.Parameters.AddWithValue("@Address", nurse.NurseAddress);
                    myCommand.Parameters.AddWithValue("@Doctor_ID", nurse.NurseDoctor);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        // delete a nurse

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM Nurse WHERE ID = @ID;";
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