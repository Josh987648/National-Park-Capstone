using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class ParkCLI
    {
        const string Command_ViewParks = "1";
        const string Command_ViewCampgrounds = "2";
        const string Command_SearchReservation = "3";
        const string Command_BookReservation = "4";
        const string Command_Exit = "5";
        readonly string DatabaseConnection = ConfigurationManager.ConnectionStrings["ParkDatabaseConnection"].ConnectionString;


        private void PrintMenu()
        {
            Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine(" 1 - Show all Parks");
            Console.WriteLine(" 2 - Show all Campgrounds");
            Console.WriteLine(" 3 - Search for Reservation");
            Console.WriteLine(" 4 - Book Reservation");
            Console.WriteLine(" 5 - Exit");
            Console.WriteLine();
        }


        public void RunCLI()
        {
            Console.WriteLine("*********************************************");
            Console.WriteLine("Welcome to National Park Campsite Reservation");
            Console.WriteLine("*********************************************");
            Console.WriteLine();
            PrintMenu();

            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_ViewParks:
                        ViewParks();
                        break;

                    case Command_ViewCampgrounds:
                        ViewCampgrounds();
                        break;

                    case Command_SearchReservation:
                        SearchReservation();
                        break;

                    case Command_BookReservation:
                        BookReservation();
                        break;

                    case Command_Exit:
                        Console.WriteLine("Thank you for using the National Park Campsite Reservation System");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }

                PrintMenu();
            }
        }


        private void ViewParks()
        {
            ParkSqlDAL parkDAL = new ParkSqlDAL(DatabaseConnection);
            List<Park> parks = parkDAL.GetAllParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine();
                    Console.WriteLine(park.Name);
                    Console.WriteLine("*****************************");
                    Console.WriteLine("Location: " + park.Location.ToString());
                    Console.WriteLine("Established: " + park.EstablishDate.ToShortDateString());
                    Console.WriteLine("Area: " + park.Area.ToString() + " sq km");
                    Console.WriteLine("Annual Visitors: " + park.Visitors.ToString("#,##0"));
                    Console.WriteLine();
                    Console.WriteLine(park.Description.ToString());
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }


        private void ViewCampgrounds()
        {
            Console.WriteLine("Please select from the following Parks to view Campgrounds: ");
            GetAllParkNames();
            Console.WriteLine();
            int parkId = int.Parse(Console.ReadLine());
            CampgroundSqlDAL campDAL = new CampgroundSqlDAL(DatabaseConnection);
            List<Campground> campgrounds = campDAL.GetAllCampgrounds(parkId);
            Console.WriteLine();
            Console.WriteLine("Park Campgrounds: ");
            Console.WriteLine("*****************************");
            Console.WriteLine("Name".ToString().PadRight(25) + "Open".ToString().PadRight(25) + "Close".ToString().PadRight(25) + "Daily Fee".ToString().PadRight(25));
            Console.WriteLine();

            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine(campground.Name.ToString().PadRight(25) + campground.OpenFrom.ToString().PadRight(25) + campground.OpenTo.ToString().PadRight(25) + Math.Round(campground.DailyFee,2).ToString().PadRight(25));
            }
            Console.WriteLine();
        }


        private void SearchReservation()
        {

        }


        private void BookReservation()
        {

        }

        private void GetAllParkNames()
        {
            ParkSqlDAL parkDAL = new ParkSqlDAL(DatabaseConnection);
            List<Park> parks = parkDAL.GetAllParks();

            foreach (Park park in parks)
            {
                Console.WriteLine(park.ParkId + ")" + " " + park.Name);
            }
        }
    }
}
