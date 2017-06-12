using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        public CampgroundSqlDAL(string parkConnectionString)
        {
            connectionString = parkConnectionString;
        }

        public List<Campground> GetAllCampgrounds(int parkId)
        {
            List<Campground> result = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Select * From Campground Where park_id = @parkId;", connection);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(PopulateCampgroundObject(reader));
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

        private Campground PopulateCampgroundObject(SqlDataReader reader)
        {
            Campground campground = new Campground();
            campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            campground.ParkId = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
            campground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return campground;
        }

        public Campground GetCampgroundById(int campgroundId)
        {
            Campground campground = new Campground();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Select * From Campground Where campground_id = @campgroundId;", connection);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                        campground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in the database" + ex.Message);
                throw;
            }
            return campground;
        }
    }
}
