using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Venue
    {
        private string _venueName;
        private int _id;

        public Venue(string venueName, int id=0)
        {
            _venueName = venueName;
            _id = id;
        }

//Get'ers and Set'ers
        public string GetVenue()
        {
          return _venueName;
        }
        public void SetVenue(string VenueName)
        {
            _venueName = VenueName;
        }

        public int GetId()
        {
          return _id;
        }

//Overide Method
        public override bool Equals(System.Object otherVenue)
        {
          if (!(otherVenue is Venue))
          {
            return false;
          }
          else
          {
             Venue newVenue = (Venue) otherVenue;
             bool idEquality = this.GetId() == newVenue.GetId();
             bool venueNameEquality = this.GetVenue() == newVenue.GetVenue();
             return (idEquality && venueNameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetVenue().GetHashCode();
        }


//SQL Table Calls

//Deletes all venues in database
        public static void DeleteAll()
        {
           MySqlConnection conn = DB.Connection();
           conn.Open();

           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"DELETE FROM venues;";

           cmd.ExecuteNonQuery();

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
        }

//Saves Venue to venues datatable
        public void SaveVenue()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO venues (venue_name) VALUES (@Venue);";

            MySqlParameter venueName = new MySqlParameter();
            venueName.ParameterName = "@Venue";
            venueName.Value = this._venueName;
            cmd.Parameters.Add(venueName);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

//Gets all venues from venue datatable
        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM venues;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int id = rdr.GetInt32(0);
              string venueName = rdr.GetString(1);
              Venue newVenue = new Venue(venueName, id);
              allVenues.Add(newVenue);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allVenues;
        }
    }
}
