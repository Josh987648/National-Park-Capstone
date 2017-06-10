using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Program
    {
        static void Main(string[] args)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ParkDatabaseConnection"].ConnectionString;
            ParkCLI cli = new ParkCLI();
            cli.RunCLI();
        }
    }
}
