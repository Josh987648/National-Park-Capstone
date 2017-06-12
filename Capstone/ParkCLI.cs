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

        string from_date;
        string to_date;

        private ParkSqlDAL parkDAL;
        private CampgroundSqlDAL campDAL;
        private ReservationSqlDAL reservationDAL;
        private SiteSqlDAL siteDAL;

        readonly string DatabaseConnection = ConfigurationManager.ConnectionStrings["ParkDatabaseConnection"].ConnectionString;

        public ParkCLI()
        {
            parkDAL = new ParkSqlDAL(DatabaseConnection);
            campDAL = new CampgroundSqlDAL(DatabaseConnection);
            reservationDAL = new ReservationSqlDAL(DatabaseConnection);
            siteDAL = new SiteSqlDAL(DatabaseConnection);
        }

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

        Campground chosenCampground;
        private void ViewCampgrounds()
        {
            List<int> campgroundIds = new List<int>();

            while (true)
            {
                Console.WriteLine("Please select from the following Parks to view Campgrounds: ");
                GetAllParkNames();
                Console.WriteLine();
                int parkId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                List<int> allParkIds = getAllParkIds();
                if (allParkIds.Contains(parkId))
                {
                    Console.WriteLine("Park Campgrounds: ");
                    Console.WriteLine("*****************************");

                    Console.WriteLine(parkDAL.GetParkNameByParkId(parkId) + " National Park:");
                    Console.WriteLine("*****************************");
                    Console.WriteLine();
                    Console.WriteLine("".PadRight(10) + "Name".ToString().PadRight(35) + "Open".ToString().PadRight(25) + "Close".ToString().PadRight(25) + "Daily Fee");
                    Console.WriteLine();

                    List<Campground> campgrounds = campDAL.GetAllCampgrounds(parkId);
                    foreach (Campground campground in campgrounds)
                    {
                        printCampground(campground);
                        campgroundIds.Add(campground.CampgroundId);
                    }
                    Console.WriteLine();
                    bool condition = true;
                    while (condition)
                    {
                        Console.WriteLine("Please select from the above Campgrounds: ");
                        userChoiceCampground = int.Parse(Console.ReadLine());
                        if (campgroundIds.Contains(userChoiceCampground))
                        {

                            chosenCampground = campDAL.GetCampgroundById(userChoiceCampground);
                            printCampground(chosenCampground);
                            condition = false;
                        }
                    }

                    Console.WriteLine();
                    SearchReservation(chosenCampground);
                    return;
                }

            }
        }

        private void SearchReservation(Campground chosenCampground)
        {
            bool condition = true;
            while (condition)
            {
                PrintSubMenu();
                string usersResponseToSubMenu = Console.ReadLine();

                if (usersResponseToSubMenu == Command_SearchReservation)
                {

                    Console.WriteLine("Please enter the arrival date:(YYYY-MM-DD) ");
                    string arrivalDate = Console.ReadLine();


                    Console.WriteLine("Please enter the departure date: (YYYY-MM-DD)");
                    string departureDate = Console.ReadLine();

                    if (!isReserved(arrivalDate, departureDate) && isDateValid(arrivalDate, departureDate, chosenCampground))
                    {
                        GetAllAvailableSites(arrivalDate, departureDate);
                        BookReservation();
                        condition = false;
                    }

                    else
                    {
                        Console.WriteLine("Sorry no site is available for that date. Please choose another date.");
                    }

                }

                else if (usersResponseToSubMenu == Command_PreviousMenu)
                {
                    return;
                }
            }
        }


        private void BookReservation()
        {
            Console.WriteLine("Which site would you like to reserve ?(enter 0 to cancel)");
            int userChoiceSite = int.Parse(Console.ReadLine());
            if (userChoiceSite == 0)
            {
                return;
            }

            Console.WriteLine("What name should the reservation be made under?");
            string reservationName = Console.ReadLine();
            reservationDAL.BookReservation(reservationName, userChoiceSite, from_date, to_date);
            Console.WriteLine("The reservation has been made and the confirmation id is " + reservationDAL.GetReservationId(reservationName));
        }

        private void GetAllParkNames()
        {

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
        }

        public void GetAllAvailableSites(string arrivalDate, string departureDate)
        {
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
        }

        public bool isReserved(string arrivalDate, string departureDate)
        {
            List<Site> sites = siteDAL.GetSiteFromCampgroundId(userChoiceCampground, arrivalDate, departureDate);
            if (sites.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void printCampground(Campground campground)
        {
            string userChoiceOpenMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campground.OpenFrom);
            string userChoiceCloseMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(campground.OpenTo);
            Console.WriteLine(campground.CampgroundId.ToString().PadRight(5) + campground.Name.ToString().PadRight(40) + userChoiceOpenMonth.ToString().PadRight(25) + userChoiceCloseMonth.ToString().PadRight(25) + "$" + Math.Round(campground.DailyFee, 2));
        }

        public List<int> getAllParkIds()
        {
            List<Park> park = parkDAL.GetAllParks();
            List<int> parkIds = new List<int>();
            for (int i = 0; i < park.Count; i++)
            {
                parkIds.Add(park[i].ParkId);
            }
            return parkIds;
        }


        public bool isDateValid(string arrivalDate, string departureDate, Campground campground)
        {
            DateTime arrival;
            arrival = Convert.ToDateTime(arrivalDate);
            if (arrival.Month >= campground.OpenFrom && arrival.Month <= campground.OpenTo)
            {
                return true;
            }
            return false;

        }


    }
}
