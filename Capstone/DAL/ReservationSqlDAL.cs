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
    }
}
