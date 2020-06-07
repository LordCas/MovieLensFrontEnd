using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MovieLensFrontEnd.Models;
using MovieLensFrontEnd.Models.IMDBjson;
using PagedList;

namespace MovieLensFrontEnd.Controllers
{
    public class MoviesController : Controller
    {
        private string IMDBjsonString { get; set; }
        private MovieLensEntities db = new MovieLensEntities();

        // GET: Movies
        public ActionResult Index( int? page, string searchString)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var movies = from m in db.Movies
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
                return View(db.Movies.Where(s => s.Title.Contains(searchString)).OrderBy(i => i.MovieId).ToPagedList(pageNumber, pageSize));
            }

            return View(db.Movies.OrderBy( i => i.MovieId ).ToPagedList(pageNumber, pageSize));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            double fiveStarsMin = 44.5;
            double fourStarsMax = 44.4;
            double fourStarsMin = 34.5;
            double threeStarsMin = 24.5;
            double threeStarsMax = 34.4;
            double twoStarsMin = 14.7;
            double twoStarsMax = 24.4;

            double? OneStars = 0;
            double? TwoStars = 0;
            double? ThreeStars = 0;
            double? FourStars = 0;
            double? FiveStars = 0;

            double? TotalUsersRated = 0;
            


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (db)
            {
                OneStars = db.Ratings.Where(s => (double)s.MovieID == id && (double)s.Rating1 <= twoStarsMin).Count();
                TwoStars = db.Ratings.Where(s => (double)s.MovieID == id && (double)s.Rating1 <= twoStarsMax && (double)s.Rating1 >= twoStarsMin).Count();
                ThreeStars = db.Ratings.Where(s => (double)s.MovieID == id && (double)s.Rating1 <= threeStarsMax && (double)s.Rating1 >= threeStarsMin).Count();
                FourStars = db.Ratings.Where(s => (double)s.MovieID == id && (double)s.Rating1 <= fourStarsMax && (double)s.Rating1 >= fourStarsMin).Count();
                FiveStars = db.Ratings.Where(s => (double)s.MovieID == id && (double)s.Rating1 >= fiveStarsMin).Count();

                if (FiveStars == 0|| FourStars == 0 || ThreeStars == 0 || TwoStars == 0 || OneStars == 0)
                {
                    FiveStars = FiveStars + 0.00000001;
                    FourStars = FourStars + 0.00000001;
                    ThreeStars = ThreeStars + 0.00000001;
                    TwoStars = TwoStars + 0.00000001;
                    OneStars = OneStars + 0.00000001;
                }
                var movieTitle = db.Database.SqlQuery<string>("GetIMDBMovieID @movieID",
                                                            new SqlParameter("@movieID", id)).First().ToString();
                string movieIDStringTT = "tt"+ movieTitle;
                var IMDBJsonResponse = GetIMDBInformation(movieIDStringTT);
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                RootObject oRootObject = new RootObject();
                oRootObject = oJS.Deserialize<RootObject>(this.IMDBjsonString);

                TotalUsersRated = db.Ratings.Count(s => s.MovieID == id);
                var model = new MovieInformationModel
                {
                    Movies = db.Movies.Where(s => s.MovieId == id).ToList(),
                    Links = db.Links.Where(s => s.MovieId == id).ToList(),
                    Ratings = db.Ratings.Where(s => s.MovieID == id).ToList(),
                    AvarageRatings = Math.Round((double)(db.Ratings.Where(s => s.MovieID == id).Average(r => r.Rating1)), 1),
                    TotalUsersWhomRated = TotalUsersRated,
                    FiveStarRatings = Math.Round((double)FiveStars, 1),
                    FourStarRatings = Math.Round((double)FourStars, 1),
                    ThreeStarRatings = Math.Round((double)ThreeStars, 1),
                    TwoStarRatings = Math.Round((double)TwoStars, 1),
                    OneStarRatings = Math.Round((double)OneStars, 1),
                    OneStarRatedPercentage = Math.Round((double)((OneStars / TotalUsersRated) * 100), 0),
                    TwoStarRatedPercentage = Math.Round((double)(TwoStars / TotalUsersRated) * 100, 0),
                    ThreeStarRatedPercentage = Math.Round((double)(ThreeStars / TotalUsersRated) * 100, 0),
                    FourStarRatedPercentage = Math.Round((double)(FourStars / TotalUsersRated) * 100, 0),
                    FiveStarRatedPercentage = Math.Round((double)(FiveStars / TotalUsersRated) * 100, 0),
                    IMDBInformation = oRootObject
                };

                if (model == null)
                {
                    return HttpNotFound();
                }

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult GetIMDBInformation(string movieID)
        {
            string text = "";
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    var IMDBjsonResponse = webClient.DownloadString("http://www.omdbapi.com/?i=" + movieID + "&apikey=fb5ad673");
                    this.IMDBjsonString = IMDBjsonResponse;
                    return Json(IMDBjsonResponse);
                }
            }
            catch (Exception e)
            {
                text = "error";
            }
            return Json(new { json = text });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
