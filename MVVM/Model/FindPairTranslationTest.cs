using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class FindPairTranslationTest : TestModel
    {
        public static Queue<FindPairTranslationTestInfo> FindPairTranslationTestInfos { get; set; }

        public FindPairTranslationTest(List<int> underThemeIds)
        {
            FindPairTranslationTestInfos = new Queue<FindPairTranslationTestInfo>();

            ConditionForIds = CreateConditionForIds(underThemeIds);

            string sqlForWords =
                $"SELECT words.id, words.word, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds};";

            SqlConnection = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for FindPairTranslationTest.");

                SqlCommand = new SqlCommand(sqlForWords, SqlConnection);

                SqlDataReader = SqlCommand.ExecuteReader();

                Words = new Queue<Word>();

                while (SqlDataReader.Read())
                {
                    Words.Enqueue(new Word
                    {
                        Id = SqlDataReader.GetInt32(0),
                        Name = SqlDataReader.GetString(1),
                        Translation = SqlDataReader.GetString(2)
                    });
                }

                Log.Info("Successfully selected words info from database for FindPairTranslationTest.");

                SqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for FindPairTranslationTest: ", ex);
            }

            try
            {
                foreach (Word currentWord in Words)
                {
                    string sqlForStruct =
                        $"SELECT TOP 3 word FROM words RIGHT JOIN words_translations ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds} AND word != '{currentWord.Name}' ORDER BY NEWID()";

                    string[] newWords = new string[3];

                    SqlCommand = new SqlCommand(sqlForStruct, SqlConnection);
                    SqlDataReader = SqlCommand.ExecuteReader();

                    int i = 0;
                    while (SqlDataReader.Read())
                    {
                        newWords[i] = SqlDataReader.GetString(0);
                        i++;
                    }

                    FindPairTranslationTestInfos.Enqueue(new FindPairTranslationTestInfo(
                        currentWord.Translation,
                        currentWord.Name,
                        newWords[0],
                        newWords[1],
                        newWords[2]
                    ));

                    SqlDataReader.Close();
                }

                Log.Info("Successfully selected pictures info from database for FindPairTranslationTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for FindPairTranslationTest: ", ex);
            }

            SqlConnection.Close();
        }
    }
}