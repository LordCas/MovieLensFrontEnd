using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieLensFrontEnd.Models;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class EuclideanDistance
    {
        public List<double> SubjectMovieRatingsModelList { get; set; }
        public List<double> OtherUserMovieRatingsModelList { get; set; }
        protected double Distance;

        public double CalculateDistance()
        {
            double result = 0;
            double innerResult = 0;

            for (int i = 0; i < OtherUserMovieRatingsModelList.Count; i++)
            {
                innerResult += Math.Pow(OtherUserMovieRatingsModelList[i] - SubjectMovieRatingsModelList[i], 2);
            }

            result = Math.Sqrt(innerResult);
            this.Distance = result;
            return result;
        }


    }

   
}