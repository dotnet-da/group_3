using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        // dependency injection to read connection strings

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"SELECT UserID, UserName FROM User";
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

        [HttpGet("{id}")]
        public JsonResult GetOne(int id)
        {
            string query = @"SELECT UserID, UserName FROM User WHERE UserID = @UserID";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@UserID", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post([FromBody] User body)
        {
            string query = @"INSERT INTO User (UserName, UserPassword) VALUES (@UserName, @UserPassword);";
            string SQLDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            DataTable table = new DataTable();
            using (MySqlConnection myConnection = new MySqlConnection(SQLDataSource))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@UserName", body.UserName);
                    myCommand.Parameters.AddWithValue("@DepartmentName", BCrypt.Net.BCrypt.HashPassword(body.UserPassword));

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
