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

//Saves Venue to venues datatable
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

//Gets all venues from venue datatable
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
    }
}
