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
        //private int parkId = 0;


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

                //cmd = new SqlCommand("Insert into park(name, location, establish_date, area, visitors, description) Values('Yosemite', 'California', '1890-10-01', 147237, 4295127, 'Yosemite features sheer granite cliffs, exceptionally tall waterfalls, and old-growth forests at a unique intersection of geology and hydrology. Half Dome and El Capitan rise from the park's centerpiece, the glacier-carved Yosemite Valley, and from its vertical walls drop Yosemite Falls, one of North America's tallest waterfalls at 2,425 feet (739 m) high. Three giant sequoia groves, along with a pristine wilderness in the heart of the Sierra Nevada, are home to a wide variety of rare plant and animal species');", connection);
                //parkId = (int)cmd.ExecuteScalar();
                //cmd.ExecuteNonQuery();
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
            Assert.AreEqual(numberOfParks, parks.Count);
        }

        [TestMethod]
        public void GetParkNameByParkIdTest()
        {
            //Arrange
            ParkSqlDAL parkSql = new ParkSqlDAL(connectionString);
            //Park park = new Park();
            //park.Name = "Yosemite";
            //park.Location = "California";
            //DateTime establishDate = new DateTime(1890, 10, 01);
            //park.EstablishDate = establishDate;
            //park.Area = 147237;
            //park.Visitors = 4295127;
            //park.Description = "Yosemite features sheer granite cliffs, exceptionally tall waterfalls, and old-growth forests at a unique intersection of geology and hydrology. Half Dome and El Capitan rise from the park's centerpiece, the glacier-carved Yosemite Valley, and from its vertical walls drop Yosemite Falls, one of North America's tallest waterfalls at 2,425 feet (739 m) high. Three giant sequoia groves, along with a pristine wilderness in the heart of the Sierra Nevada, are home to a wide variety of rare plant and animal species";
            

            //Act
            string result = parkSql.GetParkNameByParkId(4);

            //Assert
            Assert.IsNotNull(false);
            Assert.AreEqual("Yosemite", result);
        }
    }
}
