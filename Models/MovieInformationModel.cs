using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieLensFrontEnd.Models
{
    public class MovieInformationModel
    {
        public List<Models.Movy> Movies { get; set; }
        public List<Models.Link> Links { get; set; }
        public List<Models.Rating> Ratings { get; set; }
        public double? AvarageRatings { get; set; }
        public double? TotalUsersWhomRated { get; set; }
        public double? FiveStarRatings { get; set; }
        public double? FourStarRatings { get; set; }
        public double? ThreeStarRatings { get; set; }
        public double? TwoStarRatings { get; set; }
        public double? OneStarRatings { get; set; }
        public double? OneStarRatedPercentage { get; set; }
        public double? TwoStarRatedPercentage { get; set; }
        public double? ThreeStarRatedPercentage { get; set; }
        public double? FourStarRatedPercentage { get; set; }
        public double? FiveStarRatedPercentage { get; set; }
        public IMDBjson.RootObject IMDBInformation { get; set; }



    }
}