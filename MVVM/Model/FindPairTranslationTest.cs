using Filian.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class FindPairTranslationTest
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public ObservableCollection<Word> Words { get; set; }
        public static Queue<FindPairTranslationTestInfo> FindPairTranslationTestInfos { get; set; }

        public FindPairTranslationTest(List<int> underThemeIds)
        {
            FindPairTranslationTestInfos = new Queue<FindPairTranslationTestInfo>();

            string conditionForIds = "";

            foreach (int underThemeId in underThemeIds)
            {
                conditionForIds += $" theme_id = {underThemeId} OR";
            }

            conditionForIds = conditionForIds.Remove(conditionForIds.Length - 3);

            string sqlForWords =
                $"SELECT words.id, words.word, words_translations.translation FROM words Inner JOIN words_translations ON words.id = words_translations.word_id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds};";

            string sqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            try
            {
                sqlConnection.Open();

                Log.Info("Successfully connected to database for FindPairTranslationTest.");

                sqlCommand = new SqlCommand(sqlForWords, sqlConnection);

                sqlDataReader = sqlCommand.ExecuteReader();

                Words = new ObservableCollection<Word>();

                while (sqlDataReader.Read())
                {
                    Words.Add(new Word
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        Translation = sqlDataReader.GetString(2)
                    });
                }

                Log.Info("Successfully selected words info from database for FindPairTranslationTest.");

                sqlDataReader.Close();
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
                        $"SELECT TOP 3 word FROM words RIGHT JOIN words_translations ON words_translations.word_id = words.id AND language_id = {MainViewModel.LanguageId} WHERE {conditionForIds} AND word != '{currentWord.Name}' ORDER BY NEWID()";

                    string[] newWords = new string[3];

                    sqlCommand = new SqlCommand(sqlForStruct, sqlConnection);
                    sqlDataReader = sqlCommand.ExecuteReader();

                    int i = 0;
                    while (sqlDataReader.Read())
                    {
                        newWords[i] = sqlDataReader.GetString(0);
                        i++;
                    }

                    FindPairTranslationTestInfos.Enqueue(new FindPairTranslationTestInfo(
                        currentWord.Translation,
                        currentWord.Name,
                        newWords[0],
                        newWords[1],
                        newWords[2]
                    ));

                    sqlDataReader.Close();
                }

                Log.Info("Successfully selected pictures info from database for FindPairTranslationTest.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select pictures info from database for FindPairTranslationTest: ", ex);
            }

            sqlConnection.Close();
        }
    }
}