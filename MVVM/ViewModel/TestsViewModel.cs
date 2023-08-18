using System;
using Filian.Core;
using System.Collections.ObjectModel;
using Filian.MVVM.Model;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class TestsViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public ObservableCollection<Test> Tests { get; set; }

        private static Test _selectedTest;
        public Test SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }

        public TestsViewModel()
        {
            string sqlForTest = 
                $"SELECT tests.id, name, picture_path, translation FROM tests LEFT JOIN tests_translations ON tests.id = tests_translations.test_id AND tests_translations.language_id = {MainViewModel.LanguageId};";

            string sqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                Log.Info("Successfully connected to database.");

                SqlCommand sqlCommand = new SqlCommand(sqlForTest, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                Tests = new ObservableCollection<Test>();

                while (sqlDataReader.Read())
                {
                    Tests.Add(new Test
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2),
                        Translation = sqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected tests info from database.");

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select tests info from database: ", ex);
            }
        }
    }
}