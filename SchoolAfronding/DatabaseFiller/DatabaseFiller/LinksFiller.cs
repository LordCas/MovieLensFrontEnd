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
    class LinksFiller
    {
        private List<string> linksList { get; set; }
        private string connectionString { get; set; }

        public LinksFiller(List<string> linksList, string connectionString)
        {
            this.linksList = linksList;
            this.connectionString = connectionString;

        }
        public void LinksListSplitter()
        {
            linksList.RemoveAt(0);
            int regelCounter = 1;
            int identityCounter = 1;
            string movieId = "";
            string imdbID = "";
            string timdbId = "";

            var linksDataTable = new DataTable();
            linksDataTable.Columns.Add("Id");
            linksDataTable.Columns.Add("MovieId");
            linksDataTable.Columns.Add("ImdbId");
            linksDataTable.Columns.Add("TmdbId");
            foreach (var item in linksList)
            {
                var lineInWords = Regex.Split(item, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                int waardeCounter = 1;


                foreach (var word in lineInWords)
                {
                    if (waardeCounter == 1)
                    {
                        movieId = word;
                    }

                    else if (waardeCounter == 2)
                    {
                        imdbID = word;
                    }

                    else if (waardeCounter == 3)
                    {
                        timdbId = word;
                    }

                    waardeCounter++;
                }
                if (linksDataTable.Rows.Count <= 5000000)
                {
                    linksDataTable.Rows.Add(identityCounter, Int32.Parse(movieId), imdbID, timdbId);
                }
                else
                {
                    AddDBEntrySQLBulk(linksDataTable, connectionString);
                    linksDataTable.Rows.Clear();
                }
                regelCounter++;
            }
            AddDBEntrySQLBulk(linksDataTable, connectionString);
        }

        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "Links";
                sqlBulk.WriteToServer(datatable);
            }
        }
    }
}
