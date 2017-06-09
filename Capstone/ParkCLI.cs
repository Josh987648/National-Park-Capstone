using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
        int userChoiceCampground;
       
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
            Console.WriteLine("*********************************************************************");
            Console.WriteLine("           WELCOME TO THE NATIONAL PARK RESERVATION SYSTHEM          ");
            Console.WriteLine("*********************************************************************");
            Console.WriteLine();
            bool condition = true;
            while (condition)
            {
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
            List<int> campgroundIds = new List<int>();
            Console.WriteLine("Please select from the following Parks to view Campgrounds: ");
            GetAllParkNames();
            Console.WriteLine();
            int parkId = int.Parse(Console.ReadLine());

            CampgroundSqlDAL campDAL = new CampgroundSqlDAL(DatabaseConnection);
            List<Campground> campgrounds = campDAL.GetAllCampgrounds(parkId);
            Console.WriteLine();
            Console.WriteLine("Park Campgrounds: ");
            Console.WriteLine("*****************************");
            ParkSqlDAL parkDAL = new ParkSqlDAL(DatabaseConnection);
            Console.WriteLine(parkDAL.GetParkNameByParkId(parkId) + " National Park:");
            Console.WriteLine("*****************************");          
            Console.WriteLine();
            Console.WriteLine("".PadRight(10) + "Name".ToString().PadRight(35) + "Open".ToString().PadRight(25) + "Close".ToString().PadRight(25) + "Daily Fee");
            Console.WriteLine();

            foreach (Campground campground in campgrounds)
            {
                string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campground.OpenFrom);
                string closeMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campground.OpenTo);
                Console.WriteLine("#" + campground.CampgroundId.ToString().PadRight(5) + campground.Name.ToString().PadRight(40) + openMonth.ToString().PadRight(25) + closeMonth.ToString().PadRight(25) + "$" + Math.Round(campground.DailyFee,2));
                campgroundIds.Add(campground.CampgroundId);
            }
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Please select from the above Campgrounds: ");
                userChoiceCampground = int.Parse(Console.ReadLine());
                if (campgroundIds.Contains(userChoiceCampground))
                {
                    Campground campgroundChoice = campDAL.GetCampgroundById(userChoiceCampground);
                    string userChoiceOpenMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campgroundChoice.OpenFrom);
                    string userChoiceCloseMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campgroundChoice.OpenTo);
                    Console.WriteLine(campgroundChoice.CampgroundId.ToString().PadRight(5) + campgroundChoice.Name.ToString().PadRight(40) + userChoiceOpenMonth.ToString().PadRight(25) + userChoiceCloseMonth.ToString().PadRight(25) + "$" + Math.Round(campgroundChoice.DailyFee, 2));
                    
                    break;
                }
            }
            Console.WriteLine();
            PrintSubMenu();    
            
        }

        string from_date = "";
        string to_date = "";

        private void SearchReservation()
        {
            Console.WriteLine("Please enter the arrival date: ");
            string arrivalDate = Console.ReadLine();
            Console.WriteLine("Please enter the departure date: ");
            string departureDate = Console.ReadLine();
            ReservationSqlDAL reservationDAL = new ReservationSqlDAL(DatabaseConnection);
            
            if(reservationDAL.isReserved(arrivalDate, departureDate))
            {
                Console.WriteLine("Sorry that reservation is not available. Please try a new date.");
            }
            else
            {
                Console.WriteLine("Results matching your search criteria: ");
                SiteSqlDAL siteDAL = new SiteSqlDAL(DatabaseConnection);
                List<Site> sites = siteDAL.GetSiteFromCampgroundId(userChoiceCampground, arrivalDate, departureDate);
                Console.WriteLine();
                Console.WriteLine("Site No.".ToString().PadRight(25) + "Max Occup.".ToString().PadRight(25) + "Accessible".ToString().PadRight(25) + "Max RV Length".ToString().PadRight(27) + "Utilities");
                Console.WriteLine();
                from_date = arrivalDate;
                to_date = departureDate;

                foreach (Site site in sites)
                {
                    Console.WriteLine("#" + site.SiteNumber.ToString().PadRight(27) + site.MaxOccupancy.ToString().PadRight(25) + site.Accessible.ToString().PadRight(27) + site.MaxRVLength.ToString().PadRight(25) + site.Utilities);
                }


                Console.WriteLine("Which site would you like to reserve ?");
                int userChoiceSite = int.Parse(Console.ReadLine());

                Console.WriteLine("What name should the reservation be made under?");
                string reservationName = Console.ReadLine();
               reservationDAL.BookReservation(reservationName, userChoiceSite, from_date, to_date);
                Console.WriteLine("The reservation hae been made and the confirmation id is ");
            }
        }


        //private void BookReservation(ReservationSqlDAL reservation)
        //{
        //    Console.WriteLine("Which site would you like to reserve ?");
        //    int userChoiceSite = int.Parse(Console.ReadLine());

        //    Console.WriteLine("What name should the reservation be made under?");
        //    string reservationName = Console.ReadLine();
        //    reservation.BookReservation(reservationName, userChoiceSite, from_date, to_date);

        //}


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
                SearchReservation();
              
               
            }
            else if(usersResponseToSubMenu == Command_PreviousMenu)
            {
                return;
            }
        }
    }
}
