using MovieLensFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class NearestNeighbour
    {
        public List<int?> NearestNeighbourUserIDList = new List<int?>();
        private MovieLensEntities db = new MovieLensEntities();
        public double SimilarityThreshold;
        public int SubjectUserID { get; set; }
        protected double PearsonResult;
        protected double ManhattanDistance;
        protected double ManhattanSimilarity;
        protected double EuclidianDistance;
        protected double EuclidianSimilarity;
        protected double CosineResult;
        protected double MedianSimilarityResult;

        public void CalculateNearestNeighbour(int subjectUserID, double nearestNeighboursK, double threshold)
        {
            var SubjectUserID = subjectUserID;
            //var SubjectMovieRatings = db.Database.SqlQuery<NearestNeighbourModel.GetMovieRatingsByUserIdList>("GetMovieRatingsByUserIdList @userID",
            //                                                                                                    new SqlParameter("@userID",subjectUserID)).ToList();
            var UserIdsMinusSubjectIds = db.Ratings.Where(b => b.UserID != subjectUserID).Select(b => b.UserID).ToList();

            

            for (int i = 0; i < UserIdsMinusSubjectIds.Count()-1; i++)
            {
               var OtherUserMovieFilteredRatingsList = db.Database.SqlQuery<double>("GetUserMoviesRatingsUserIdFilteredVsTargetUser @targetUser, @userID", 
                                                                                new SqlParameter("@targetUser", UserIdsMinusSubjectIds[i]), 
                                                                                new SqlParameter("@userID", subjectUserID)).ToList();
               var SubjectMovieFilteredRatingsList = db.Database.SqlQuery<double>("GetUserMoviesRatingsTargetUserIdFilteredVsUserID  @targetUser, @userID",
                                                                                new SqlParameter("@targetUser", UserIdsMinusSubjectIds[i]),
                                                                                new SqlParameter("@userID", subjectUserID)).ToList();





                GetManthattan(SubjectMovieFilteredRatingsList, OtherUserMovieFilteredRatingsList);
                GetEuclidian(SubjectMovieFilteredRatingsList, OtherUserMovieFilteredRatingsList);
                GetSimilarity();
                GetCosine(SubjectMovieFilteredRatingsList, OtherUserMovieFilteredRatingsList);
                GetPearson(SubjectMovieFilteredRatingsList, OtherUserMovieFilteredRatingsList);



                //Debug.WriteLine("TargetUserID : " + UserIdsMinusSubjectIds[i] + " / " + UserIdsMinusSubjectIds.Count + 
                //                "\nSubjectUserID : " + subjectUserID + 
                //                "\nEuclidian Distance : " +this.EuclidianDistance +
                //                "\nEuclidian Similarity : " + this.EuclidianSimilarity +
                //                "\nManhattan Distance : " +this.ManhattanDistance +
                //                "\nManhattan Similarity: " +this.ManhattanSimilarity +
                //                "\nPearson Coefficient Correlation: " + this.PearsonResult +
                //                "\nCosine Similarity: " + this.CosineResult )  ; 

                if (threshold <= this.PearsonResult)
                {
                    NearestNeighbourUserIDList.Add(UserIdsMinusSubjectIds[i]);
                    if (NearestNeighbourUserIDList.Count == nearestNeighboursK)
                    {
                        break;
                    }
                }
            }
        }

        private double GetCosine(List<double> SubjectMovieFilteredRatingsList, List<double> OtherUserMovieFilteredRatingsList)
        {
            double result = 0;

            var cosine = new CosineSimilarity
            {
                SubjectMovieRatingsModelList = SubjectMovieFilteredRatingsList,
                OtherUserMovieRatingsModelList = OtherUserMovieFilteredRatingsList
            };

            this.CosineResult = cosine.CalculateCosineSimilarity();
            return result;
        }

        private void GetManthattan(List<double> SubjectMovieFilteredRatingsList, List<double> OtherUserMovieFilteredRatingsList)
        {
            var manhattanDistance = new ManhattenDistance
            {
                SubjectMovieRatingsModelList = SubjectMovieFilteredRatingsList,
                OtherUserMovieRatingsModelList = OtherUserMovieFilteredRatingsList

            };
            this.ManhattanDistance = manhattanDistance.ManhattanDistance();
        }

        private void GetEuclidian(List<double> SubjectMovieFilteredRatingsList, List<double> OtherUserMovieFilteredRatingsList)
        {
            var euclidian = new EuclideanDistance
            {
                SubjectMovieRatingsModelList = SubjectMovieFilteredRatingsList,
                OtherUserMovieRatingsModelList = OtherUserMovieFilteredRatingsList
            };
            this.EuclidianDistance = euclidian.CalculateDistance();
        }

        private void GetPearson(List<double> SubjectMovieFilteredRatingsList, List<double> OtherUserMovieFilteredRatingsList)
        {
            var pearson = new PearsonCoeficient
            {
                SubjectMovieRatingsModelList = SubjectMovieFilteredRatingsList,
                OtherUserMovieRatingsModelList = OtherUserMovieFilteredRatingsList
            };
            this.PearsonResult = pearson.CalculatePearson();
        }

        private void GetSimilarity()
        {
            this.EuclidianSimilarity = 1 / (1 + this.EuclidianDistance);
            this.ManhattanSimilarity = 1 / (1 + this.ManhattanDistance);
        }



    }
}