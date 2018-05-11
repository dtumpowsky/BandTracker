using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BandTracker.Tests
{
    [TestClass]
    public class VenueTests : IDisposable
    {
        public VenueTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
        }

        public void Dispose()
        {
          Venue.DeleteAll();
        }

        [TestMethod]
        public void VenueTable_is_Empty()
        {
            int result = Venue.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_Venue_Database()
        {
            Venue testVenue = new Venue("Showbox");
            testVenue.SaveVenue();

            List<Venue> result = Venue.GetAll();
            List<Venue> testList = new List<Venue>{testVenue};

            CollectionAssert.AreEqual(testList, result);
        }
    }
}
