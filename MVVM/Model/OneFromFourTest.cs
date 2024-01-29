using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromFourTest : TestModel
    {
        public static Queue<OneFromFourTestInfo> OneFromFourTestInfos { get; set; }

        public OneFromFourTest(List<int> underThemeIds)
        {
            OneFromFourTestInfos = new Queue<OneFromFourTestInfo>();

            ConditionForIds = CreateConditionForIds(underThemeIds);

            string sqlForWords =
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds};";

            SqlConnection = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for OneFromFourTest.");

                SqlCommand = new SqlCommand(sqlForWords, SqlConnection);

                SqlDataReader = SqlCommand.ExecuteReader();

                Words = new Queue<Word>();

                while (SqlDataReader.Read())
                {
                    Words.Enqueue(new Word
                    {
                        Id = SqlDataReader.GetInt32(0),
                        Name = SqlDataReader.GetString(1),
                        PicturePath = SqlDataReader.GetString(2),
                        Translation = SqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected words info from database for OneFromFourTest.");

                SqlDataReader.Close();
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

                        string sqlForStruct = "SELECT TOP 3 picture_path FROM words WHERE (" + ConditionForIds +
                                              $") AND picture_path != '{word.PicturePath}' " +
                                              "ORDER BY NEWID()";

                        string[] picPaths = new string[3];
                        SqlCommand = new SqlCommand(sqlForStruct, SqlConnection);
                        SqlDataReader = SqlCommand.ExecuteReader();
                        int i = 0;
                        while (SqlDataReader.Read())
                        {
                            picPaths[i] = SqlDataReader.GetString(0);
                            i++;
                        }

                        SqlDataReader.Close();

                        OneFromFourTestInfos.Enqueue(new OneFromFourTestInfo(
                            word.Name,
                            word.Translation,
                            word.PicturePath,
                            picPaths[0],
                            picPaths[1],
                            picPaths[2]
                        ));

                        SqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromFourTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromFourTest: ", ex);
            }

            SqlConnection.Close();
        }
    }
}