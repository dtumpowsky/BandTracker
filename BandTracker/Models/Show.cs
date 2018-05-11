using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Show
    {
        private int _venueId;
        private int _bandId;
        private int _id;

        public Show(int venueId, int bandId, int id=0)
        {
            _venueId = venueId;
            _bandId = bandId;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }
        public int GetVenueId()
        {
            return _venueId;
        }
        public int GetBandId()
        {
            return _bandId;
        }

        public override bool Equals(System.Object otherShow)
        {
          if (!(otherShow is Show))
          {
            return false;
          }
          else
          {
             Show newShow = (Show) otherShow;
             bool idEquality = this.GetId() == newShow.GetId();
             bool idVenueEquality = this.GetVenueId() == newShow.GetVenueId();
             bool idBandEquality = this.GetBandId() == newShow.GetBandId();
             return (idEquality && idVenueEquality && idBandEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetId().GetHashCode();
        }

        //deletes from join table
        public void DeleteShow()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM shows WHERE show_id = @ShowId;";

            MySqlParameter showIdParameter = new MySqlParameter();
            showIdParameter.ParameterName = "@ShowId";
            showIdParameter.Value = _id;
            cmd.Parameters.Add(showIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
              conn.Close();
            }

            conn.Dispose();
        }

        public static Show Find(int showId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM shows WHERE show_id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = showId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            int venueId = 0;
            int bandId = 0;

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              venueId = rdr.GetInt32(1);
              bandId = rdr.GetInt32(2);
            }
            Show newShow= new Show(venueId, bandId, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newShow;
        }
    }
}
