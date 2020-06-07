using MovieLensFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class RecommendTopMovies
    {
        private List<int?> UserIDList = new List<int?>();
        protected List<string> RecommendedMovieTitles = new List<string>();
        protected List<string> RecommendedMovieTitlesTemp = new List<string>();
        public List<string> FilteredRecommendedMovieTitles = new List<string>();
        private MovieLensEntities db = new MovieLensEntities();
        public void GetTop5RandomRecommendedMovies(int subjectUser, int k, double threshold, int TopAmountOfMovies)
        {
            var nearestNeighbourK = new RecommenderSystem.NearestNeighbour();
            nearestNeighbourK.CalculateNearestNeighbour(subjectUser, k, threshold);
            UserIDList = nearestNeighbourK.NearestNeighbourUserIDList;

            for (int i = 0; i < UserIDList.Count; i++)
            {
                RecommendedMovieTitles = db.Database.SqlQuery<string>("GetTop5RecommondedMovies  @UserID, @TargetUserID",
                                                                        new SqlParameter("@UserID", subjectUser),
                                                                        new SqlParameter("@TargetUserID", UserIDList[i])).ToList();
            }

            RecommendedMovieTitles.Distinct();

            for (int i = 0; i < TopAmountOfMovies; i++)
            {
                if(i >= RecommendedMovieTitles.Count)
                {
                    break;
                }
                FilteredRecommendedMovieTitles.Add(RecommendedMovieTitles[i]);
            }
        }
    }
}