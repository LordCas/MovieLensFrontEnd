using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieLensFrontEnd.Models;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class CosineSimilarity
    {
        public List<double> SubjectMovieRatingsModelList { get; set; }
        public List<double> OtherUserMovieRatingsModelList { get; set; }
        protected double Similarity;

        public double CalculateCosineSimilarity()
        {
            double result = 0;

            double dotProduct = 0;
            double vectorLentghSubject = 0;
            double vectorLentghOtherUser = 0;
            double vectorV = 0;
            for (int i = 0; i < OtherUserMovieRatingsModelList.Count(); i++)
            {
                dotProduct += SubjectMovieRatingsModelList[i] * OtherUserMovieRatingsModelList[i];
            }


            for (int i = 0; i < OtherUserMovieRatingsModelList.Count(); i++)
            {
                vectorLentghSubject += Math.Pow(OtherUserMovieRatingsModelList[i],2);
                vectorLentghOtherUser += Math.Pow(OtherUserMovieRatingsModelList[i], 2);
            }

            vectorLentghSubject = Math.Sqrt(vectorLentghSubject);
            vectorLentghOtherUser = Math.Sqrt(vectorLentghOtherUser);
            vectorV = vectorLentghSubject * vectorLentghOtherUser;
            result = dotProduct / vectorV;
            this.Similarity = result;
            return result;
        }
    }
}