using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MovieLensFrontEnd.Models;
using System.Collections;
using System.Data.SqlClient;

namespace MovieLensFrontEnd.Controllers
{
    public class UsersController : Controller
    {
        private MovieLensEntities db = new MovieLensEntities();

        // GET: Users
        public ActionResult Index(int? page)
        {
            //    int pageSize = 12;
            //    int pageNumber = (page ?? 1);
            var UserAvarageRatingsAndTotalRatingsList = db.Database.SqlQuery<Models.UserAvarageRatingsAndTotalRatingsModel>("GetUserIDsRoundedAvarageRatingsAndTotalRatings").ToList();

            var model = new UserRatingsInformationModel
            {
                UserAvarageRatingsAndTotalRatings = UserAvarageRatingsAndTotalRatingsList
            };

            return View(model);
        }
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }

            //int? moviesPerUser = (int?)db.Ratings.Where(s => s.UserID == id).Select(b => b.MovieID);
            string lastPlacedRating = db.Ratings.Where(i => i.UserID == id).OrderByDescending(i => i.Timestamp).Select(i=> i.Timestamp).First().ToString();
            string firstPlacedRating = db.Ratings.Where(i => i.UserID == id).OrderBy(i => i.Timestamp).Select(i => i.Timestamp).First().ToString() ;
            int totalRatingsGiven = db.Ratings.Where(i => i.UserID == id).Select(i => i.Rating1).Count();
            var fullFilteredRatingList = db.Ratings.Where(a => a.UserID == id).ToList();
            var filteredRatinglist = db.Movies.Join(
                                        db.Ratings,
                                        b => b.MovieId,
                                        r4ting => r4ting.MovieID,
                                        (b, r4ting) => new { r4ting.Rating1, r4ting.UserID, r4ting.MovieID }).Where(b => b.UserID == id).Select(a => a.Rating1).ToList();
            var filteredMovieList = db.Movies.Join(
                                        db.Ratings,
                                        b => b.MovieId,
                                        r4ting => r4ting.MovieID,
                                        (b, r4ting) => new { b.MovieId, r4ting.UserID}).Where(b => b.UserID == id).Select(a => a.MovieId).ToList();
            var filteredMovieTitlesList = db.Movies.Join(
                                       db.Ratings,
                                       b => b.MovieId,
                                       r4ting => r4ting.MovieID,
                                       (b, r4ting) => new { r4ting.UserID, b.Title }).Where(b => b.UserID == id).Select(a => a.Title).ToList();
            var clientUserIdParameter = new SqlParameter("@userID", id);
            var UserMoviesRatingsByUserIDList = db.Database.SqlQuery<TimeLineModel>("GetUserMoviesRatingsByUserID @userID", clientUserIdParameter).ToList();
            var Top5Movies = new RecommenderSystem.RecommendTopMovies();
            Top5Movies.GetTop5RandomRecommendedMovies((int)id, 5, 0.23, 5);

            var top5MoviesList = Top5Movies.FilteredRecommendedMovieTitles;
            using (db)
            {

                var model = new UserRatingsInformationModel
                {
                    Ratings = fullFilteredRatingList,
                    Movies = db.Movies.ToList(),
                    MoviesRatedByUser = filteredMovieList,
                    MovieTitles = filteredMovieTitlesList,
                    RatingsWithMatchingMovies = filteredRatinglist,
                    Tags = db.Tags.Where(i => i.UserId == id).ToList(),
                    LastPlacedRating = lastPlacedRating,
                    TotalRatingsGiven = totalRatingsGiven,
                    FirstPlacedRating = firstPlacedRating,
                    UserMoviesRatingsByUserID = UserMoviesRatingsByUserIDList,
                    TopRecommendedMovies = top5MoviesList
                };

                return View(model);
            }
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
