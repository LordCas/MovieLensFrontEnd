using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.Models
{
    public class UserRatingsInformationModel
    {
        public List<Models.Movy> Movies { get; set; }
        public List<Models.Rating> Ratings { get; set; }
        public List<Models.Tag> Tags { get; set; }

        public List<string> TopRecommendedMovies { get; set; }
        public List<int?> MoviesRatedByUser { get; set; }
        public List<string> MovieTitles { get; set; }
        public List<double?> RatingsWithMatchingMovies { get; set; }
        public List<UserAvarageRatingsAndTotalRatingsModel> UserAvarageRatingsAndTotalRatings { get; set; }
        public List<string> TimeLineDatesAscending { get; set; }
        public List<TimeLineModel> UserMoviesRatingsByUserID { get; set; }

        public string FirstPlacedRating { get; set; }
        public string LastPlacedRating { get; set; }
        public int TotalRatingsGiven { get; set; }

    }

    public partial class TimeLineModel
    {
        public string Genres { get; set; }
        public string Title { get; set; }
        public double? Rating { get; set; }
        public DateTime? TimeStamp { get; set; }
    }

    public partial class UserAvarageRatingsAndTotalRatingsModel
    {
        public int? UserID { get; set; }
        public double? AvarageRating { get; set; }
        public int? TotalRatings { get; set; }
    }
}