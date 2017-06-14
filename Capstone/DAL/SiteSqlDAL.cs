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

        public List<Site> GetSiteFromCampgroundId(int campground_Id, DateTime from_date, DateTime to_date)
        {
            List<Site> result = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Top 5 * " +
                        "FROM site " +
                        "INNER JOIN campground on site.campground_id = campground.campground_id " +
                        "Where site_id NOT IN" +
                        " (select reservation.site_id " +
                        "FROM reservation " +
                        "INNER JOIN site on reservation.site_id = site.site_id " +
                        "WHERE @from_date < reservation.to_date and @to_date > reservation.from_date)" +
                        " AND campground.campground_id = @campground_Id;", connection); 
                    
                    cmd.Parameters.AddWithValue("@campground_Id", campground_Id);
                    cmd.Parameters.AddWithValue("@from_date", from_date);
                    cmd.Parameters.AddWithValue("@to_date", to_date);
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
