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
    class GenomeTagsFiller
    {
        private List<string> genomeTagsList { get; set; }
        private string connectionString { get; set; }

        public GenomeTagsFiller(List<string> linksList, string connectionString)
        {
            this.genomeTagsList = linksList;
            this.connectionString = connectionString;

        }
        public void GenomeTagListSplitter()
        {
            genomeTagsList.RemoveAt(0);
            string tag = "";
            string tagId = "";
            int identityCounter = 1;
            var genomeTagsDataTable = new DataTable();
            genomeTagsDataTable.Columns.Add("Id");
            genomeTagsDataTable.Columns.Add("TagId");
            genomeTagsDataTable.Columns.Add("Tag");

            genomeTagsDataTable.Columns[0].DataType = typeof(Int32);
            try
            {
                foreach (var item in genomeTagsList)
                {
                    var lineInWords = Regex.Split(item, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    int waardeCounter = 1;

                    foreach (var word in lineInWords)
                    {
                        if (waardeCounter == 1)
                        {
                            tagId = word;
                        }

                        else if (waardeCounter == 2)
                        {
                            tag = word;
                        }

                        waardeCounter++;
                    }

                    if (genomeTagsDataTable.Rows.Count <= 5000000)
                    {
                        genomeTagsDataTable.Rows.Add(identityCounter,Int32.Parse(tagId), tag);
                    }
                    else
                    {
                        AddDBEntrySQLBulk(genomeTagsDataTable, connectionString);
                        genomeTagsDataTable.Rows.Clear();
                    }
                }
                AddDBEntrySQLBulk(genomeTagsDataTable, connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

           // Environment.Exit(0);
        }

        public void AddDBEntrySQLBulk(DataTable datatable, string connectionString)
        {
            using (var sqlBulk = new SqlBulkCopy(connectionString))
            {
                sqlBulk.NotifyAfter = 1000000;
                sqlBulk.SqlRowsCopied += (sender, eventArgs) => Console.WriteLine("Wrote " + eventArgs.RowsCopied + " records.");
                sqlBulk.DestinationTableName = "GenomeTags";
                sqlBulk.WriteToServer(datatable);
            }
        }
    }
}
