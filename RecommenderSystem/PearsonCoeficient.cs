using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class PearsonCoeficient
    {
        public List<double> SubjectMovieRatingsModelList { get; set; }
        public List<double> OtherUserMovieRatingsModelList { get; set; }
        protected double Correlation;

        public double CalculatePearson()
        {
            
            double result = 0;

            if (SubjectMovieRatingsModelList.Count == 0 || OtherUserMovieRatingsModelList.Count == 0)
            {
                Correlation = 1;
                return 1;
            }

            double subjectMovieRatingsAvarage = SubjectMovieRatingsModelList.Average();
            double otherUserMovieRatingsAvarage = OtherUserMovieRatingsModelList.Average();
            double upperPartPearsonCoefficient = 0;
            double subjectLowerPartPearsonCoefficientPow = 0;
            double subjectLowerPartPearsonCoefficientSquareroot= 0;
            double otherUserLowerPartPearsonCoefficientPow = 0;
            double otherUserLowerPartPearsonCoefficientSqareroot = 0;

            for (int i = 0; i < SubjectMovieRatingsModelList.Count(); i++)
            {
                upperPartPearsonCoefficient += (SubjectMovieRatingsModelList[i] - subjectMovieRatingsAvarage) * (OtherUserMovieRatingsModelList[i] - otherUserMovieRatingsAvarage);
            }

            for (int i = 0; i < SubjectMovieRatingsModelList.Count(); i++)
            {
                subjectLowerPartPearsonCoefficientPow += Math.Pow( ( SubjectMovieRatingsModelList[i] - subjectMovieRatingsAvarage ), 2 );
            }

            subjectLowerPartPearsonCoefficientSquareroot = Math.Sqrt(subjectLowerPartPearsonCoefficientPow);

            for (int i = 0; i < OtherUserMovieRatingsModelList.Count(); i++)
            {
                otherUserLowerPartPearsonCoefficientPow += Math.Pow((OtherUserMovieRatingsModelList[i] - otherUserMovieRatingsAvarage), 2);
            }

            otherUserLowerPartPearsonCoefficientSqareroot = Math.Sqrt(otherUserLowerPartPearsonCoefficientPow);

            result = upperPartPearsonCoefficient / (subjectLowerPartPearsonCoefficientSquareroot * otherUserLowerPartPearsonCoefficientSqareroot);
            this.Correlation = result;

            return result;
        }
    }
}