using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatabaseFiller
{
    class MoviesFiller
    {
        private List<string> moviesList { get; set; }
        private string connectionString { get; set; }

        public MoviesFiller(List<string> movies, string connectionString)
        {
            this.moviesList = movies;
            this.connectionString = connectionString;
        }

        public void MovieListSplitter()
        {
            moviesList.RemoveAt(0);
            string movieID = "";
            string title = "";
            string genres = "";
            int identityCounter = 1;

            var moviesDataTable = new DataTable();
            moviesDataTable.Columns.Add("Id");
            moviesDataTable.Columns.Add("MovieId");
            moviesDataTable.Columns.Add("Title");
            moviesDataTable.Columns.Add("Genres");

            moviesDataTable.Columns[0].DataType = typeof(Int32);
            foreach (var item in moviesList)
            {
                var lineInWords = Regex.Split(item, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                int waardeCounter = 1;


                foreach (var word in lineInWords)
                {
                    if (waardeCounter == 1)
                    {
                        movieID = word;
                    }

                    else if (waardeCounter == 2)
                    {
                        title = word;
                    }

                    else if (waardeCounter == 3)
                    {
                        genres = word;
                    }

                    waardeCounter++;
                }
                if (moviesDataTable.Rows.Count <= 5000000)
                {
                    moviesDataTable.Rows.Add(identityCounter,Int32.Parse(movieID), title, genres);
                }
                else
                {
                    AddDBEntrySQLBulk(moviesDataTable, connectionString);
                    moviesDataTable.Rows.Clear();
                }
            }
            AddDBEntrySQLBulk(moviesDataTable, connectionString);
        }

        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "Movies";
                sqlBulk.WriteToServer(datatable);
            }
        }
    }
}
