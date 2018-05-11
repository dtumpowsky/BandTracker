using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class BandController : Controller
    {
        [HttpGet("/bands")]
        public ActionResult Index()
        {
            List<Band> allBands = Band.GetAll();
            return View(allBands);
        }
        [HttpGet("/bands/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-new-band")]
        public ActionResult Create()
        {
            Band newBand = new Band(Request.Form["band-name"]);
            newBand.SaveBand();
            return View("Success", "Home");
        }

        [HttpGet("/bands/{id}")]
        public ActionResult ViewBands(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Band selectedBand = Band.Find(id);
            List<Show> shows = selectedBand.GetVenueBandIsPlaying();
            List<Venue> allVenues = Venue.GetAll();
            model.Add("selectedBand", selectedBand);
            model.Add("shows", shows);
            model.Add("allVenues", allVenues);
            return View(model);
        }
        [HttpPost("/bands/{Id}/venues/new")]
        public ActionResult AddVenueToBand(int Id)
        {
            Band band = Band.Find(Id);
            Venue venue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
            band.SetVenueForBandToPlay(venue);
            return RedirectToAction("ViewBands",  new { id = Id });
        }

        [HttpGet("/bands/{id}/cancel")]
        public ActionResult Cancel(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Show selectedShow = Show.Find(id);
            int bandId = selectedShow.GetBandId();
            selectedShow.DeleteShow();
            return RedirectToAction("ViewBands",  new { id = bandId });


        }
    }
}
