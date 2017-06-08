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
        const string Command_Exit = "3";
        const string Command_SearchReservation = "1";
        const string Command_PreviousMenu = "2";
        readonly string DatabaseConnection = ConfigurationManager.ConnectionStrings["ParkDatabaseConnection"].ConnectionString;


        private void PrintMainMenu()
        {
            Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine(" 1 - Show all Parks");
            Console.WriteLine(" 2 - Show all Campgrounds");
            Console.WriteLine(" 3 - Exit");
            Console.WriteLine();
        }


        public void RunCLI()
        {
            Console.WriteLine("*******************************************************");
            Console.WriteLine("Welcome to the National Park Campsite Reservation System");
            Console.WriteLine("*******************************************************");
            Console.WriteLine();
            PrintMainMenu();

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


                    case Command_Exit:
                        Console.WriteLine("Thank you for using the National Park Campsite Reservation System");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
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
            int counter = 1;
            CampgroundSqlDAL campDAL = new CampgroundSqlDAL(DatabaseConnection);
            List<Campground> campgrounds = campDAL.GetAllCampgrounds(parkId);
            Console.WriteLine();
            Console.WriteLine("Park Campgrounds: ");
            Console.WriteLine("*****************************");
            ParkSqlDAL parkDAL = new ParkSqlDAL(DatabaseConnection);
            Console.WriteLine(parkDAL.GetParkNameByParkId(parkId) + " National Park:");
            Console.WriteLine("*****************************");
            Console.WriteLine("Please select from the following Campgrounds: ");
            Console.WriteLine();
            Console.WriteLine("".PadRight(10) + "Name".ToString().PadRight(35) + "Open".ToString().PadRight(25) + "Close".ToString().PadRight(25) + "Daily Fee");
            Console.WriteLine();

            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine("#" + counter.ToString().PadRight(5) + campground.Name.ToString().PadRight(40) + campground.OpenFrom.ToString().PadRight(25) + campground.OpenTo.ToString().PadRight(25) + "$" + Math.Round(campground.DailyFee,2));
                counter++;
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

        private void PrintSubMenu()
        {
            Console.WriteLine("Menu Please type in a command");
            Console.WriteLine("1 - Search for available Reservation");
            Console.WriteLine("2 - Return to previous screen");
            string usersResponseToSubMenu = Console.ReadLine();
            
            if (usersResponseToSubMenu == Command_SearchReservation)
            {

            }
        }

        private void SearchForAvailableReservation()
        {
            
        }
    }
}
