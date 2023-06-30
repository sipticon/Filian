using Filian.MVVM.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromTwoTest
    {
        private static Queue<OneFromTwoStruct> oneFromTwoStructs;
        public ObservableCollection<Word> Words { get; set; }
        public static Queue<OneFromTwoStruct> OneFromTwoStructs
        {
            get => oneFromTwoStructs;
            set => oneFromTwoStructs = value;
        }

        public OneFromTwoTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            OneFromTwoStructs = new Queue<OneFromTwoStruct>();
            foreach (int underThemeID in mainViewModel.UnderthemesId)
            {
                string sqlForWords = $" SELECT * FROM words WHERE theme_id = {underThemeID}";
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
                        PicturePath = sqlDataReader.GetString(2), 
                        Name = sqlDataReader.GetString(1) });

                }
                sqlDataReader.Close();
                foreach (Word word in Words)
                {
                    if (word.PicturePath.Contains("'"))
                        word.PicturePath = word.PicturePath.Replace("'", "''");

                    string sqlForStruct = "SELECT TOP 1 picture_path FROM words " +
                                  $"WHERE theme_id = {underThemeID} AND picture_path != '{word.PicturePath}' " +
                                  "ORDER BY NEWID()";

                    sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                    sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        OneFromTwoStructs.Enqueue(new OneFromTwoStruct(
                            word.Name,
                            word.PicturePath,
                            sqlDataReader.GetString(0)
                        ));
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
           
        }
         
    }
}
