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
    class GenomeScoresFiller
    {
        private List<string> genomeScoresList { get; set; }
        private string connectionString { get; set; }
        
        public GenomeScoresFiller(List<string> linksList, string connectionString)
        {
            this.genomeScoresList = linksList;
            this.connectionString = connectionString;

        }
        public void GenomeScoreListSplitter()
        {
            genomeScoresList.RemoveAt(0);
            string movieId = "";
            string tagId = "";
            string relevance = "";
            int identityCounter = 1;

            var genomeScoreDataTable = new DataTable();
            genomeScoreDataTable.Columns.Add("Id");
            genomeScoreDataTable.Columns.Add("MovieId");
            genomeScoreDataTable.Columns.Add("TagId");
            genomeScoreDataTable.Columns.Add("Relevance");

            genomeScoreDataTable.Columns[0].DataType = typeof(Int32);
            try
            {
                foreach (var item in genomeScoresList)
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
                            tagId = word;
                        }

                        else if (waardeCounter == 3)
                        {
                            relevance = word;
                        }

                        waardeCounter++;
                    }

                    if(genomeScoreDataTable.Rows.Count <= 5000000)
                    {
                        genomeScoreDataTable.Rows.Add(identityCounter,Int32.Parse(movieId), Int32.Parse(tagId), relevance);
                    } 
                    else
                    {
                        AddDBEntrySQLBulk(genomeScoreDataTable, connectionString);
                        genomeScoreDataTable.Rows.Clear();
                    }
                    //AddDBUserEntry(movieId, tagId, relevance, connectionString);
                }
                AddDBEntrySQLBulk(genomeScoreDataTable, connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            Environment.Exit(0);
        }

        //sql statements 1-voor-1 importen kost teveel in tijd
        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "GenomeScores";
                sqlBulk.WriteToServer(datatable);
            }
        }
    }
}
