using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Filian.MVVM.ViewModel;

namespace Filian.MVVM.Model
{
    public class OneFromTwoTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private static Queue<OneFromTwoTestInfo> _oneFromTwoTestInfos;
        public ObservableCollection<Word> Words { get; set; }
        public static Queue<OneFromTwoTestInfo> OneFromTwoTestInfos
        {
            get => _oneFromTwoTestInfos;
            set => _oneFromTwoTestInfos = value;
        }

        public OneFromTwoTest(List<int> underThemeIds)
        {
            _oneFromTwoTestInfos = new Queue<OneFromTwoTestInfo>();

            string conditionForIds = "";

            foreach (int underThemeId in underThemeIds)
            {
                conditionForIds += $" theme_id = {underThemeId} OR";
            }

            conditionForIds = conditionForIds.Remove(conditionForIds.Length - 3);

            string sqlForWords = 
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds} ;";

            string sqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            try
            {
                sqlConnection.Open();

                Log.Info("Successfully connected to database for OneFromTwoTest.");

                sqlCommand = new SqlCommand(sqlForWords, sqlConnection);

                sqlDataReader = sqlCommand.ExecuteReader();

                Words = new ObservableCollection<Word>();

                while (sqlDataReader.Read())
                {
                    Words.Add(new Word
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2),
                        Translation = sqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected words info from database for OneFromTwoTest.");

                sqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for OneFromTwoTest: ", ex);
            }

            try
            {
                if (Words != null)
                    foreach (Word word in Words)
                    {
                        if (word.PicturePath.Contains("'"))
                            word.PicturePath = word.PicturePath.Replace("'", "''");

                        string sqlForStruct = "SELECT TOP 1 picture_path FROM words WHERE (" + conditionForIds +
                                              $") AND picture_path != '{word.PicturePath}' " +
                                              "ORDER BY NEWID()";

                        sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);

                        sqlDataReader = sqlCommand.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            _oneFromTwoTestInfos.Enqueue(new OneFromTwoTestInfo(
                                word.Name,
                                word.Translation,
                                word.PicturePath,
                                sqlDataReader.GetString(0)
                            ));
                        }

                        sqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromTwoTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromTwoTest: ", ex);
            }

            sqlConnection.Close();
        }
        
    }
}