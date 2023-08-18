using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class TrueOrFalseTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public ObservableCollection<Word> Words { get; set; }
        public static Queue<TrueOrFalseTestInfo> TrueOrFalseTestInfos { get; set; }

        public TrueOrFalseTest(List<int> underThemeIds)
        {
            TrueOrFalseTestInfos = new Queue<TrueOrFalseTestInfo>();

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

                Log.Info("Successfully connected to database for TrueOrFalseTest.");

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

                Log.Info("Successfully selected words info from database for TrueOrFalseTest.");

                sqlDataReader.Close();
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
                            $"SELECT TOP 1 translation FROM words_translations RIGHT JOIN words ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds} AND translation != '{currentWordTranslation}' ORDER BY NEWID()";

                        sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                        sqlDataReader = sqlCommand.ExecuteReader();
                        if (sqlDataReader.Read())
                        {
                            TrueOrFalseTestInfos.Enqueue(new TrueOrFalseTestInfo(
                                currentWord.Name,
                                currentWord.Translation,
                                currentWord.PicturePath,
                                sqlDataReader.GetString(0)
                            ));
                        }

                        sqlDataReader.Close();
                    }

                Log.Info("Successfully selected words info from database for TrueOrFalseTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select words info from database for TrueOrFalseTest: ", ex);
            }

            sqlConnection.Close();
        }
    }
}