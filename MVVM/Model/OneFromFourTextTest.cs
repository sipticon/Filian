using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class OneFromFourTextTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public ObservableCollection<Word> Words { get; set; }
        public static Queue<OneFromFourTextTestInfo> OneFromFourTextTestInfos { get; set; }

        public OneFromFourTextTest(List<int> underThemeIds)
        {
            OneFromFourTextTestInfos = new Queue<OneFromFourTextTestInfo>();

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

                Log.Info("Successfully connected to database for OneFromFourTextTest.");

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

                Log.Info("Successfully selected words info from database for OneFromFourTextTest.");

                sqlDataReader.Close();
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
                            $"SELECT TOP 3 translation FROM words_translations RIGHT JOIN words ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds} AND translation != '{currentWordTranslation}' ORDER BY NEWID()";

                        string[] newWords = new string[3];

                        sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                        sqlDataReader = sqlCommand.ExecuteReader();

                        int i = 0;
                        while (sqlDataReader.Read())
                        {
                            newWords[i] = sqlDataReader.GetString(0);
                            i++;
                        }

                        OneFromFourTextTestInfos.Enqueue(new OneFromFourTextTestInfo(
                            currentWord.Translation,
                            newWords[0],
                            newWords[1],
                            newWords[2],
                            currentWord.PicturePath
                        ));

                        sqlDataReader.Close();
                    }

                Log.Info("Successfully selected pictures info from database for OneFromFourTextTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for OneFromFourTextTest: ", ex);
            }

            sqlConnection.Close();
        }
    }
}