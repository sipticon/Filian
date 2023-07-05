using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromTwoTest
    {
        private static Queue<OneFromTwoTestInfo> oneFromTwoTestInfos;
        public ObservableCollection<Word> Words { get; set; }
        public static Queue<OneFromTwoTestInfo> OneFromTwoTestInfos
        {
            get => oneFromTwoTestInfos;
            set => oneFromTwoTestInfos = value;
        }

        public OneFromTwoTest(List<int> underthemeIds)
        {
            oneFromTwoTestInfos = new Queue<OneFromTwoTestInfo>();

            string conditionForIds = "";

            foreach (int underThemeId in underthemeIds)
            {
                conditionForIds += $" theme_id = {underThemeId} OR";
            }

            conditionForIds = conditionForIds.Remove(conditionForIds.Length - 3);

            string sqlForWords = " SELECT * FROM words WHERE"+conditionForIds;

            string ssqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(ssqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlForWords, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Words = new ObservableCollection<Word>();

            while (sqlDataReader.Read())
            {
                Words.Add(new Word { 
                    Id = sqlDataReader.GetInt32(0),
                    Name = sqlDataReader.GetString(1) ,
                    Picture_Path = sqlDataReader.GetString(2)
                });

            }
            sqlDataReader.Close();
            foreach (Word word in Words)
            {
                if (word.Picture_Path.Contains("'"))
                    word.Picture_Path = word.Picture_Path.Replace("'", "''");

                string sqlForStruct = "SELECT TOP 1 picture_path FROM words WHERE (" + conditionForIds+
                              $") AND picture_path != '{word.Picture_Path}' " +
                              "ORDER BY NEWID()";

                sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    oneFromTwoTestInfos.Enqueue(new OneFromTwoTestInfo(
                        word.Name,
                        word.Picture_Path,
                        sqlDataReader.GetString(0)
                    ));
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
        }
         
    }
}
