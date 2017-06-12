using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System.Transactions;
using System.Data.SqlClient;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests.DALTests
{
    [TestClass]
    public class ParkSqlDALTests
    {
        private TransactionScope trans;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;User ID=te_student;Password=sqlserver1";
        private int numberOfParks = 0;
        private int parkId = 0;

        [TestInitialize]
        public void Initialize()
        {
            trans = new TransactionScope();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("Select Count(*) From Park;", connection);
                numberOfParks = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description ) Values('Yosemite', 'California', '1890-10-01', 147237, 4295127, 'Yosemite features sheer granite cliffs, exceptionally tall waterfalls, and old-growth forests at a unique intersection of geology and hydrology. Half Dome and El Capitan rise from the park centerpiece, the glacier-carved Yosemite Valley, and from its vertical walls drop Yosemite Falls'); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                parkId = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            trans.Dispose();
        }

        [TestMethod]
        public void GetAllParksTest()
        {
            //Arrange
            ParkSqlDAL parkSql = new ParkSqlDAL(connectionString);

            //Act
            List<Park> parks = parkSql.GetAllParks();

            //Assert
            Assert.IsNotNull(parks);
            Assert.AreEqual(numberOfParks + 1, parks.Count);
        }

        [TestMethod]
        public void GetParkNameByParkIdTest()
        {
            //Arrange
            ParkSqlDAL parkSql = new ParkSqlDAL(connectionString);

            //Act
            string result = parkSql.GetParkNameByParkId(parkId);

            //Assert
            Assert.IsNotNull(false);
            Assert.AreEqual("Yosemite", result);
        }
    }
}
