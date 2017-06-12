using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests.DALTests
{
    [TestClass]
    public class SiteSqlDALTests
    {
        private TransactionScope trans;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;User ID=te_student;Password=sqlserver1";
        int numberOfSites;

        [TestInitialize]
        public void Initialize()
        {
            trans = new TransactionScope();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();
                cmd = new SqlCommand("SELECT Top 5 * FROM site INNER JOIN campground on site.campground_id = campground.campground_id " +
                    "Where site_id not in (select reservation.site_id " +
                    "from reservation inner join site on reservation.site_id = site.site_id " +
                    "where '2017-06-06' < reservation.to_date and '2017-06-09' > reservation.from_date) " +
                    "and campground.campground_id = 1; ", connection);
                numberOfSites = (int)cmd.ExecuteScalar();
            }
        }

        [TestMethod]
        public void GetSiteFromCampgroundId()
        {
            //Arrange
            SiteSqlDAL siteDal = new SiteSqlDAL(connectionString);

            //Act
            List<Site> listOfSites = siteDal.GetSiteFromCampgroundId(1, "2017-06-06", "2017-06-09");

            //Assert
            Assert.IsNotNull(true);
            Assert.AreEqual(numberOfSites, listOfSites.Count);
        }

        [TestMethod]
        public void GetSiteFromCampgroundWhenCampgroundIdIs6()
        {
            //Arrange
            SiteSqlDAL siteDal = new SiteSqlDAL(connectionString);

            //Act
            List<Site> listOfSites = siteDal.GetSiteFromCampgroundId(6, "2017-08-11", "2017-08-12");

            //Assert
            Assert.IsNotNull(true);
            Assert.AreEqual(1, listOfSites.Count);
        }

        [TestCleanup]
        public void CleanUp()
        {
            trans.Dispose();
        }
    }
}
