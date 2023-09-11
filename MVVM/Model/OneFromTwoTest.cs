using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Filian.MVVM.ViewModel;

namespace Filian.MVVM.Model
{
    public class OneFromTwoTest : TestModel
    {
        public static Queue<OneFromTwoTestInfo> OneFromTwoTestInfos { get; set; }

        public OneFromTwoTest(List<int> underThemeIds)
        {
            OneFromTwoTestInfos = new Queue<OneFromTwoTestInfo>();

            ConditionForIds = CreateConditionForIds(underThemeIds);

            string sqlForWords = 
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds} ;";

            SqlConnection = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for OneFromTwoTest.");

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

                Log.Info("Successfully selected words info from database for OneFromTwoTest.");

                SqlDataReader.Close();
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

                        string sqlForStruct = "SELECT TOP 1 picture_path FROM words WHERE (" + ConditionForIds +
                                              $") AND picture_path != '{word.PicturePath}' " +
                                              "ORDER BY NEWID()";

                        SqlCommand = new SqlCommand(sqlForStruct, SqlConnection);

                        SqlDataReader = SqlCommand.ExecuteReader();

                        while (SqlDataReader.Read())
                        {
                            OneFromTwoTestInfos.Enqueue(new OneFromTwoTestInfo(
                                word.Name,
                                word.Translation,
                                word.PicturePath,
                                SqlDataReader.GetString(0)
                            ));
                        }

                        SqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromTwoTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromTwoTest: ", ex);
            }

            SqlConnection.Close();
        }
    }
}