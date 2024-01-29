using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class TrueOrFalseTest : TestModel
    {
        public static Queue<TrueOrFalseTestInfo> TrueOrFalseTestInfos { get; set; }

        public TrueOrFalseTest(List<int> underThemeIds)
        {
            TrueOrFalseTestInfos = new Queue<TrueOrFalseTestInfo>();
            
            ConditionForIds = CreateConditionForIds(underThemeIds);

            string sqlForWords =
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds};";
            
            SqlConnection = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for TrueOrFalseTest.");

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

                Log.Info("Successfully selected words info from database for TrueOrFalseTest.");

                SqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for TrueOrFalseTest: ", ex);
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
                            $"SELECT TOP 1 translation FROM words_translations RIGHT JOIN words ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds} AND translation != '{currentWordTranslation}' ORDER BY NEWID()";

                        SqlCommand = new SqlCommand(sqlForStruct, SqlConnection);
                        SqlDataReader = SqlCommand.ExecuteReader();
                        if (SqlDataReader.Read())
                        {
                            TrueOrFalseTestInfos.Enqueue(new TrueOrFalseTestInfo(
                                currentWord.Name,
                                currentWord.Translation,
                                currentWord.PicturePath,
                                SqlDataReader.GetString(0)
                            ));
                        }

                        SqlDataReader.Close();
                    }

                Log.Info("Successfully selected words info from database for TrueOrFalseTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for TrueOrFalseTest: ", ex);
            }

            SqlConnection.Close();
        }
    }
}