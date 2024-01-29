using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class SpellWithVoiceTest : TestModel
    {
        public SpellWithVoiceTest(List<int> underThemeIds)
        {
            ConditionForIds = CreateConditionForIds(underThemeIds);

            string sqlForWords =
                $"SELECT words.id, words.word, words.picture_path, words_translations.translation, words_translations.path_pronounce FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {ConditionForIds} ;";

            SqlConnection  = new SqlConnection(SqlConnectionString);
            try
            {
                SqlConnection.Open();

                Log.Info("Successfully connected to database for SpellWithVoiceTest.");

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
                        Translation = SqlDataReader.GetString(3),
                        PronunciationPath = SqlDataReader.GetString(4)
                    });
                }

                Log.Info("Successfully selected words info from database for SpellWithVoiceTest.");

                SqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for SpellWithVoiceTest: ", ex);
            }
            SqlConnection.Close();
        }
    }
}