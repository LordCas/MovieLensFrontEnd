using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieLensFrontEnd.Models;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class ManhattenDistance
    {
        public List<double> SubjectMovieRatingsModelList { get; set; }
        public List<double> OtherUserMovieRatingsModelList { get; set; }
        public double Distance;
        //lists worden niet geparkeerd
        public double ManhattanDistance()
        {
            double result = 0;
            double innerCalculation = 0;

            for (int i = 0; i < OtherUserMovieRatingsModelList.Count; i++)
            {
                innerCalculation = OtherUserMovieRatingsModelList[i] - SubjectMovieRatingsModelList[i];

                if (innerCalculation < 0)
                {
                    innerCalculation *= -1;
                }

                result += innerCalculation;
            }
            Distance = result;
            return result;
        }
    }

    public static class ListExtensionsManhattan
    {
        public static List<NearestNeighbourModel.GetUserMoviesRatingsUserIdFilteredVsTargetUser> SortByManhattan(this NearestNeighbourModel.GetUserMoviesRatingsUserIdFilteredVsTargetUser input)
        {
            var newList = new List<NearestNeighbourModel.GetUserMoviesRatingsUserIdFilteredVsTargetUser>();



            return newList;
        }
    }


}