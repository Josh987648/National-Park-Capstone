using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;

        public ParkSqlDAL(string parkConnectionString)
        {
            connectionString = parkConnectionString;
        }

        public List<Park> GetAllParks()
        {
            List<Park> result = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Select * From Park;", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(PopulateParkObject(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in the database" + ex.Message);
                throw;
            }
            return result;
        }

        private Park PopulateParkObject(SqlDataReader reader)
        {
            Park park = new Park();
            park.ParkId = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }

        public string GetParkNameByParkId(int parkId)
        {
            string park = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("Select name From Park Where park_id = @park_id;", connection);
                cmd.Parameters.AddWithValue("@park_id", parkId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    park = Convert.ToString(reader["name"]);
                }
            }
            return park;
        }
    }
}
