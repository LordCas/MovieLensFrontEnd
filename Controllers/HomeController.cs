using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieLensFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult Maths()
        {
            var Top5Movies = new RecommenderSystem.RecommendTopMovies();
            Top5Movies.GetTop5RandomRecommendedMovies(820,5,0.23, 5);

            ViewBag.Message = "";
            return View();
        }
    }
}