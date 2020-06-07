using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.Models
{
    public class NearestNeighbourModel
    {
        public List<GetUserMoviesRatingsUserIdFilteredVsTargetUser> list1 { get; set; }
        public List<GetUserMoviesRatingsTargetUserFilteredVsUserId> list2 { get; set; }
        public partial class GetMovieRatingsByUserIdList
        {
            public int UserRating { get; set; }
        }

        public partial class GetUserMoviesRatingsUserIdFilteredVsTargetUser
        {
            public double FilteredUserRating { get; set; }
        }

        public partial class GetUserMoviesRatingsTargetUserFilteredVsUserId
        {
            public double FilteredUserRating { get; set; }
        }
    }
}