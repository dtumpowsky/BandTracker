using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Band
    {
        private string _bandName;
        private int _id;

        public Band(string bandName, int id=0)
        {
            _bandName = bandName;
            _id = id;
        }

//Get'ers and Set'ers
        public string GetBand()
        {
          return _bandName;
        }
        public void SetVenue(string BandName)
        {
            _bandName = BandName;
        }

        public int GetId()
        {
          return _id;
        }

//Overide Method
        public override bool Equals(System.Object otherBand)
        {
          if (!(otherBand is Band))
          {
            return false;
          }
          else
          {
             Band newBand = (Band) otherBand;
             bool idEquality = this.GetId() == newBand.GetId();
             bool bandNameEquality = this.GetBand() == newBand.GetBand();
             return (idEquality && bandNameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetBand().GetHashCode();
        }


//SQL Table Calls

//Deletes all bands in database
        public static void DeleteAll()
        {
           MySqlConnection conn = DB.Connection();
           conn.Open();

           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"DELETE FROM bands;";

           cmd.ExecuteNonQuery();

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
        }

//Saves Band to bands datatable
        public void SaveBand()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO bands (band_name) VALUES (@Band);";

            MySqlParameter bandName = new MySqlParameter();
            bandName.ParameterName = "@Band";
            bandName.Value = this._bandName;
            cmd.Parameters.Add(bandName);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

//Gets all bands from bands datatable
        public static List<Band> GetAll()
        {
            List<Band> allBands = new List<Band> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int id = rdr.GetInt32(0);
              string bandName = rdr.GetString(1);
              Band newBand = new Band(bandName, id);
              allBands.Add(newBand);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBands;
        }

//Find a particular band from table
        public static Band Find(int bandId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = bandId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string bandName = "";

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              bandName = rdr.GetString(1);
            }
            Band newBand = new Band(bandName, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBand;
        }

//get venue the band is playing at from jointable
        public List<Show> GetVenueBandIsPlaying()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            cmd.CommandText = @"SELECT * FROM shows WHERE band_id = @ThisBand;";


            MySqlParameter bandIdParameter = new MySqlParameter();
            bandIdParameter.ParameterName = "@ThisBand";
            bandIdParameter.Value = _id;
            cmd.Parameters.Add(bandIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Show> upcomingShows = new List<Show>{};

            while(rdr.Read())
            {
              int showId = rdr.GetInt32(0);
              int venueId = rdr.GetInt32(1);
              int bandId = rdr.GetInt32(2);
              Show newUpcomingShow = new Show(venueId, bandId, showId);
              upcomingShows.Add(newUpcomingShow);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return upcomingShows;
        }

        public void SetVenueForBandToPlay(Venue newVenue)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO shows (venue_id, band_id) VALUES (@VenueId, @BandId);";

            MySqlParameter venue_id = new MySqlParameter();
            venue_id.ParameterName = "@VenueId";
            venue_id.Value = newVenue.GetId();
            cmd.Parameters.Add(venue_id);

            MySqlParameter band_id = new MySqlParameter();
            band_id.ParameterName = "@BandId";
            band_id.Value = _id;
            cmd.Parameters.Add(band_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
