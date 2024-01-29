using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromFourTextTest : TestModel
    {
        public static Queue<OneFromFourTextTestInfo> OneFromFourTextTestInfos { get; set; }

        public OneFromFourTextTest(List<int> underThemeIds)
        {
            OneFromFourTextTestInfos = new Queue<OneFromFourTextTestInfo>();

            ConditionForIds = CreateConditionForIds(underThemeIds);
            string sqlForWords =
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds};";

            SqlConnection = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for OneFromFourTextTest.");

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

                Log.Info("Successfully selected words info from database for OneFromFourTextTest.");

                SqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for OneFromFourTextTest: ", ex);
            }

            try
            {
                if (Words != null)
                    foreach (Word currentWord in Words)
                    {
                        string currentWordTranslation = currentWord.Translation;
                        if (currentWordTranslation.Contains("'"))
                            currentWordTranslation = currentWordTranslation.Replace("'", "''");

                        string sqlForStruct =
                            $"SELECT TOP 3 translation FROM words_translations RIGHT JOIN words ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds} AND translation != '{currentWordTranslation}' ORDER BY NEWID()";

                        string[] newWords = new string[3];

                        SqlCommand = new SqlCommand(sqlForStruct, SqlConnection);
                        SqlDataReader = SqlCommand.ExecuteReader();

                        int i = 0;
                        while (SqlDataReader.Read())
                        {
                            newWords[i] = SqlDataReader.GetString(0);
                            i++;
                        }

                        OneFromFourTextTestInfos.Enqueue(new OneFromFourTextTestInfo(
                            currentWord.Translation,
                            newWords[0],
                            newWords[1],
                            newWords[2],
                            currentWord.PicturePath
                        ));

                        SqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromFourTextTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromFourTextTest: ", ex);
            }

            SqlConnection.Close();
        }
    }
}