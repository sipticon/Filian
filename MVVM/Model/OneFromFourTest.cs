using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromFourTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public ObservableCollection<Word> Words { get; set; }
        public static Queue<OneFromFourTestInfo> OneFromFourTestInfos { get; set; }

        public OneFromFourTest(List<int> underThemeIds)
        {
            OneFromFourTestInfos = new Queue<OneFromFourTestInfo>();

            string conditionForIds = "";

            foreach (int underThemeId in underThemeIds)
            {
                conditionForIds += $" theme_id = {underThemeId} OR";
            }

            conditionForIds = conditionForIds.Remove(conditionForIds.Length - 3);

            string sqlForWords =
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds};";

            string sqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            try
            {
                sqlConnection.Open();

                Log.Info("Successfully connected to database for OneFromFourTest.");

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

                Log.Info("Successfully selected words info from database for OneFromFourTest.");

                sqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for OneFromFourTest: ", ex);
            }

            try
            {
                if (Words != null)
                    foreach (Word word in Words)
                    {
                        if (word.PicturePath.Contains("'"))
                            word.PicturePath = word.PicturePath.Replace("'", "''");

                        string sqlForStruct = "SELECT TOP 3 picture_path FROM words WHERE (" + conditionForIds +
                                              $") AND picture_path != '{word.PicturePath}' " +
                                              "ORDER BY NEWID()";

                        string[] picPaths = new string[3];
                        sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                        sqlDataReader = sqlCommand.ExecuteReader();
                        int i = 0;
                        while (sqlDataReader.Read())
                        {
                            picPaths[i] = sqlDataReader.GetString(0);
                            i++;
                        }

                        sqlDataReader.Close();

                        OneFromFourTestInfos.Enqueue(new OneFromFourTestInfo(
                            word.Name,
                            word.Translation,
                            word.PicturePath,
                            picPaths[0],
                            picPaths[1],
                            picPaths[2]
                        ));

                        sqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromFourTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromFourTest: ", ex);
            }

            sqlConnection.Close();
        }
    }
}