using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;

        public ReservationSqlDAL(string parkConnectionString)
        {
            connectionString = parkConnectionString;
        }

        public bool isReserved(string arrivalDate, string departureDate)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("Select from_date, to_date From Reservation Where from_date = @from_date and to_date = @to_date;", connection);
                    cmd.Parameters.AddWithValue("@from_date", arrivalDate);
                    cmd.Parameters.AddWithValue("@to_date", departureDate);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in the database" + ex.Message);
                throw;
            }
            return false;
        }


        public bool BookReservation(string name, int site_id, string from_date, string to_date)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date) Values(@site_id, @name, @from_date, @to_date );", connection);
                    cmd.Parameters.AddWithValue("@site_id", site_id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@from_date", from_date);
                    cmd.Parameters.AddWithValue("@to_date", to_date);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in the database" + ex.Message);
                throw;
            }

        }
    }
}
