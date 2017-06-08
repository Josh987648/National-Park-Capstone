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
            }
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
    }
}
