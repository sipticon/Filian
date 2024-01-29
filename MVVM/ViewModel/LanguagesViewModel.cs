using System;
using Filian.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class LanguagesViewModel : ViewModel
    {
        public ObservableCollection<Language> Languages { get; set; }

        private static Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }

        public LanguagesViewModel()
        {
            string sqlForLanguages = " SELECT * FROM languages";
            
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                Log.Info("Successfully connected to database.");

                SqlCommand sqlCommand = new SqlCommand(sqlForLanguages, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                Languages = new ObservableCollection<Language>();

                while (sqlDataReader.Read())
                {
                    Languages.Add(new Language
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2)
                    });
                }

                Log.Info("Successfully selected languages info from database.");

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select languages info from database: ",ex);
            }
            
        }
    }
}