using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests.DALTests
{
    [TestClass]
    public class ReservationSqlDALTests
    {
        private TransactionScope trans;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;User ID=te_student;Password=sqlserver1";
        int reservationId;

        [TestInitialize]
        public void Initialize()
        {
            trans = new TransactionScope();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();
                cmd = new SqlCommand("INSERT into reservation(name, site_id, from_date, to_date) VALUES('xyz', 2, '2017-08-12', '2017-09-14'); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                reservationId = (int)cmd.ExecuteScalar();

            }
        }

        [TestMethod]
        public void BookReservationTest()
        {
            //Arrange
            ReservationSqlDAL reservationDal = new ReservationSqlDAL(connectionString);

            //Act
            bool result;
            result = reservationDal.BookReservation("abc", 2, DateTime.Parse("2017-09-09"), DateTime.Parse("2017-09-11"));

            //Assert
            Assert.AreEqual(true, result);

        }

        [TestCleanup]
        public void CleanUp()
        {
            trans.Dispose();
        }


        [TestMethod]
        public void GetReservationIdTest()
        {
            //Arrange
            ReservationSqlDAL reservationDal = new ReservationSqlDAL(connectionString);

            //Act
            int reservation_id = reservationDal.GetReservationId("xyz");

            //Assert
            Assert.AreEqual(reservationId, reservation_id);
        }
    }
}
