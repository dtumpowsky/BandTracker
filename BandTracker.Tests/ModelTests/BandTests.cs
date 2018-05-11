using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BandTracker.Tests
{
    [TestClass]
    public class BandTests : IDisposable
    {
        public BandTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
        }

        public void Dispose()
        {
          Band.DeleteAll();
        }

        [TestMethod]
        public void BandTable_is_Empty()
        {
            int result = Band.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_Band_Database()
        {
            Band testBand = new Band("Greensky Bluegrass");
            testBand.SaveBand();

            List<Band> result = Band.GetAll();
            List<Band> bandList = new List<Band>{testBand};

            CollectionAssert.AreEqual(bandList, result);
        }
    }
}
