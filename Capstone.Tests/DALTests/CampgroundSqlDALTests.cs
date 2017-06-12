using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Tests.DALTests
{
    [TestClass]
    public class CampgroundSqlDALTests
    {

        private TransactionScope trans;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;User ID=te_student;Password=sqlserver1";
        int numberOfCampgrounds;
        int campground_id;

        [TestInitialize]
        public void Initialize()
        {
            trans = new TransactionScope();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT count(*) from campground where park_id = 1;", connection);
                numberOfCampgrounds = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT into campground(park_id, name, open_from_mm, open_to_mm, daily_fee)" +
                    " VALUES(1,'Wawona', 1, 12, 25.00); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                campground_id = (int)cmd.ExecuteScalar();

            }
        }


        [TestMethod]
        public void GetAllCampgroundsTest()
        {
            //Arrange
            CampgroundSqlDAL campgroundDal = new CampgroundSqlDAL(connectionString);

            //Act
            List<Campground> allCampgrounds = campgroundDal.GetAllCampgrounds(1);

            //Assert
            Assert.IsNotNull(true);
            Assert.AreEqual(numberOfCampgrounds + 1, allCampgrounds.Count);
        }

        [TestCleanup]
        public void CleanUp()
        {
            trans.Dispose();
        }

        [TestMethod]
        public void GetCampgroundByIdTest()
        {
            //Arrange
            CampgroundSqlDAL campgroundDal = new CampgroundSqlDAL(connectionString);

            //Act
            Campground campground = new Campground();
            campground = campgroundDal.GetCampgroundById(campground_id);

            //Assert
            Assert.IsNotNull(true);
            Assert.AreEqual("Wawona", campground.Name);
            Assert.AreEqual(1, campground.OpenFrom);
            Assert.AreEqual(12, campground.OpenTo);
            Assert.AreEqual(25.00M, campground.DailyFee);
        }

    }
}
