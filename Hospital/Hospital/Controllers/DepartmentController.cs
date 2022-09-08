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
    public class DepartmentController : ControllerBase
    {
        // dependency injection to read connection strings

        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // get all departments

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"SELECT Department_ID, Department_Name FROM Department";
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

        // get one department

        [HttpGet("{id}")]
        public JsonResult GetOne(int id)
        {
            string query = @"SELECT Department_ID, Department_Name FROM Department WHERE Department_ID = @Department_ID";
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

        // get all doctors in a department

        [HttpGet("{id}")]
        public JsonResult GetDoctors(int id)
        {
            string query = @"SELECT ID, Name, Surname FROM Department INNER JOIN Doctor ON Doctor.Department_ID = @Department.Department_ID";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Department.Department_ID", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        // add a new department

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"INSERT INTO Department (Department_Name) VALUES (@Department_Name);";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Department_Name", department.DepartmentName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        // update a department

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"UPDATE Department SET Department_Name = @DepartmentName WHERE Department_ID = @Department_ID;";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@Department_ID", department.DepartmentID);
                    myCommand.Parameters.AddWithValue("@Department_Name", department.DepartmentName);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        // delete a department

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM Department WHERE Department_ID = @Department_ID;";
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

            return new JsonResult("Deleted Successfully");
        }

    }
}