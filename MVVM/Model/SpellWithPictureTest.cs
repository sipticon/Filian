using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class SpellWithPictureTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public static Queue<Word> Words { get; set; }

        public SpellWithPictureTest(List<int> underThemeIds)
        {
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

                Log.Info("Successfully connected to database for SpellWithPictureTest.");

                sqlCommand = new SqlCommand(sqlForWords, sqlConnection);

                sqlDataReader = sqlCommand.ExecuteReader();

                Words = new Queue<Word>();

                while (sqlDataReader.Read())
                {
                    Words.Enqueue(new Word
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2),
                        Translation = sqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected words info from database for SpellWithPictureTest.");

                sqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for SpellWithPictureTest: ", ex);
            }
            sqlConnection.Close();
        }
    }
}