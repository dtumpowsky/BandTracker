using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class VenueController : Controller
    {
        [HttpGet("/venues")]
        public ActionResult Index()
        {
            List<Venue> allVenues = Venue.GetAll();
            return View(allVenues);
        }
        [HttpGet("/venues/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-new-venue")]
        public ActionResult Create()
        {
            Venue newVenue = new Venue(Request.Form["venue-name"]);
            newVenue.SaveVenue();
            return View("Success", "Home");
        }

        [HttpGet("/venues/{id}")]
        public ActionResult ViewVenues(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Venue selectedVenue = Venue.Find(id);
            List<Show> shows = selectedVenue.GetBandsPlayingAtVenue();
            List<Band> allBands = Band.GetAll();
            model.Add("selectedVenue", selectedVenue);
            model.Add("shows", shows);
            model.Add("allBands", allBands);
            return View(model);
        }
        [HttpPost("/venues/{Id}/bands/new")]
        public ActionResult AddBandToVenue(int Id)
        {
            Venue venue = Venue.Find(Id);
            Band band = Band.Find(Int32.Parse(Request.Form["band-id"]));
            venue.SetBandToPlayAtVenue(band);
            return RedirectToAction("ViewVenues",  new { id = Id });
        }

        [HttpGet("/venues/{id}/cancel")]
        public ActionResult Cancel(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Show selectedShow = Show.Find(id);
            int venueId = selectedShow.GetVenueId();
            selectedShow.DeleteShow();
            return RedirectToAction("ViewVenues",  new { id = venueId });
        }

        [HttpGet("/venues/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            return View();
        }
        [HttpPost("update-this-venue")]
        public ActionResult Update(int id)
        {
          string newVenueName = Request.Form["venue-name-update"];
          Venue newVenue = new Venue(newVenueName, id);
          newVenue.UpdateVenue(newVenueName);
          return RedirectToAction("ViewVenues");
        }
    }
}
