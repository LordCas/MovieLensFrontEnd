using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.RecommenderSystem
{
    public class ManhattanEuclidianSimilarity
    {
        public double Distance { get; set; }
        public double Similarity { get; set; }
        public double CalculateSimilarity()
        {
            double result = 1 / (1 + Distance);
            this.Similarity = result;
            return result;
        }

    }
}