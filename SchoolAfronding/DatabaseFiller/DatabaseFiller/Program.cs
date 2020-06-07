using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatabaseFiller
{
    class Program
    {
        //dev connectionstring: data source=DESKTOP-0GJQS2G;initial catalog=MovieLens;trusted_connection=true
        static void Main(string[] args)
        {
            CheckConnection();
            string filePathMoviesData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/movies.csv";
            string filePathLinksData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/links.csv";
            string filePathRatingsData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/ratings.csv";
            string filePathTagsData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/tags.csv";
            string filePathGenomeTagsData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/genome-tags.csv";
            string filePathGenomeScoresData = "D:/projects/.NET projects/SchoolAfronding/ml-20m/genome-scores.csv";
            string connectionString =
            "Data Source=OVSPC461;" +
            "Initial Catalog=MovieLens;" +
            "trusted_connection=true;";

            //var DBFillLinksData = new LinksFiller(CheckContent(filePathLinksData), connectionString);
            //var DBFillMovieData = new MoviesFiller(CheckContent(filePathMoviesData), connectionString);
            //var DBFillRatingdata = new RatingsFiller(CheckContent(filePathRatingsData), connectionString);
            var DBFillGenomeScoresData = new GenomeScoresFiller(CheckContent(filePathGenomeScoresData), connectionString);
           // var DBFillGenomeTagsData = new GenomeTagsFiller(CheckContent(filePathGenomeTagsData), connectionString);
            DBFillGenomeScoresData.GenomeScoreListSplitter();
            //DBFillGenomeTagsData.GenomeTagListSplitter();
            //DBFillMovieData.MovieListSplitter();
            //DBFillLinksData.LinksListSplitter();

            Console.ReadLine();
        }

        public static void CheckConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
            "Data Source=OVSPC461;" +
            "Initial Catalog=MovieLens;" +
            "trusted_connection=true;";
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(conn.State);
            conn.Close();
            Console.WriteLine(conn.State);
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            var list = new List<string>();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum += count;  // sum is a buffer offset for next reading
                    Console.WriteLine("CSV bezig met inladen");
                }
                    
            }
            finally
            {
                fileStream.Close();
                Console.WriteLine("CSV klaar met inladen");
            }
            return buffer;
        }

        public static List<string> CheckContent(string filePath)
        {
            var list = new List<string>();
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    Console.WriteLine("CSV bezig met inladen");
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                        
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            Console.WriteLine("CSV klaar met inladen");
            return list;
        }
    }
}
