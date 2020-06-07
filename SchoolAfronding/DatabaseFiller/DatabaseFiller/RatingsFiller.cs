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
    class RatingsFiller
    {
        private List<string> ratingsList { get; set; }
        private string connectionString { get; set; }

        public RatingsFiller(List<string> ratings, string connectionString)
        {
            this.ratingsList = ratings;
            this.connectionString = connectionString;
        }

        public void RatingListSplitter()
        {
            ratingsList.RemoveAt(0);
            int identityCounter = 1;
            int regelCounter = 1;
            DateTime date;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            string movieID = "";
            string userID = "";
            string rating = "";
            string timestamp = "";
            var ratingsDataTable = new DataTable();
            ratingsDataTable.Columns.Add("Id");
            ratingsDataTable.Columns.Add("UserId");
            ratingsDataTable.Columns.Add("MovieId");
            ratingsDataTable.Columns.Add("Rating");
            ratingsDataTable.Columns.Add("Timestamp");

            foreach (var item in ratingsList)
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
                        userID = word;
                    }

                    else if (waardeCounter == 3)
                    {
                        rating = word;
                    }

                    else if (waardeCounter == 4)
                    {
                        timestamp = word;
                    }

                    waardeCounter++;
                }
                if (ratingsDataTable.Rows.Count <= 5000000)
                {
                    date = epoch.AddSeconds(double.Parse(timestamp));

                    ratingsDataTable.Rows.Add(identityCounter, Int32.Parse(userID), Int32.Parse(movieID), float.Parse(rating), date);
                }
                else
                {
                    AddDBEntrySQLBulk(ratingsDataTable, connectionString);
                    ratingsDataTable.Rows.Clear();
                }
                regelCounter++;
            }
            AddDBEntrySQLBulk(ratingsDataTable, connectionString);

            Environment.Exit(0);
        }

        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "Ratings";
                sqlBulk.WriteToServer(datatable);
            }
        }

    }
}
