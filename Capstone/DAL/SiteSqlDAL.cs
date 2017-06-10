using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;

        public SiteSqlDAL(string parkConnectionString)
        {
            connectionString = parkConnectionString;
        }

        public List<Site> GetSiteFromCampgroundId(int campgroundId, string arrival, string departure)
        {
            List<Site> result = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site INNER JOIN campground on site.campground_id = campground.campground_id Where site_id not in (select reservation.site_id from reservation inner join site on reservation.site_id = site.site_id where @from_date < reservation.to_date and @to_date > reservation.from_date) and campground.campground_id = @campground_id;", connection);                 
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@from_date", arrival);
                    cmd.Parameters.AddWithValue("@to_date", departure);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(PopulateSiteObject(reader));
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

        private Site PopulateSiteObject(SqlDataReader reader)
        {
            Site site = new Site();
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);

            return site;
        }
    }
}
