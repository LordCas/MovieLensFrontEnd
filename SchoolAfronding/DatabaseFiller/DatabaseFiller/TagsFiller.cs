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
    

    class TagsFiller
    {
        private List<string> tagsList { get; set; }
        private string connectionString { get; set; }

        public TagsFiller(List<string> usersList, string connectionString )
        {
            this.tagsList = usersList;
            this.connectionString = connectionString;
            
        }
        public void TagsListSplitter ()
        {
            tagsList.RemoveAt(0);
            int identityCounter = 1;
            int regelCounter = 1;
            string userID = "";
            string movieID = "";
            string tag = "";
            string timestamp = "";
            DateTime date;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            var tagsDataTable = new DataTable();
            tagsDataTable.Columns.Add("Id");
            tagsDataTable.Columns.Add("UserId");
            tagsDataTable.Columns.Add("MovieId");
            tagsDataTable.Columns.Add("Tag");
            tagsDataTable.Columns.Add("Timestamp");
            foreach (var item in tagsList)
            {
                var lineInWords = Regex.Split(item, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                int waardeCounter = 1;


                foreach (var word in lineInWords)
                {
                    if (waardeCounter == 1)
                    {
                        userID = word;
                    }

                    else if (waardeCounter == 2)
                    {
                        movieID = word;
                    }

                    else if (waardeCounter == 3)
                    {
                        tag = word;
                    }

                    else if (waardeCounter == 4)
                    {
                        timestamp = word;
                    }
                    waardeCounter++;
                }
                if (tagsDataTable.Rows.Count <= 5000000)
                {
                    date = epoch.AddSeconds(double.Parse(timestamp));

                    tagsDataTable.Rows.Add(identityCounter, Int32.Parse(userID), Int32.Parse(movieID), tag, date);
                }
                else
                {
                    AddDBEntrySQLBulk(tagsDataTable, connectionString);
                    tagsDataTable.Rows.Clear();
                }
                regelCounter++;
            }
            AddDBEntrySQLBulk(tagsDataTable, connectionString);

            Environment.Exit(0);
        }

        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "Tags";
                sqlBulk.WriteToServer(datatable);
            }
        }
    }

}
