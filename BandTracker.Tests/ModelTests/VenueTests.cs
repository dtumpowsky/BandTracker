using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BandTracker.Tests
{
    [TestClass]
    public class VenueTests
    {

        public VenueTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker;";
        }

        [TestMethod]
        public void VenueTable_is_Empty()
        {
            int result = Venue.GetAll().Count;

            Assert.AreEqual(0, result);
        }
    }
}    
