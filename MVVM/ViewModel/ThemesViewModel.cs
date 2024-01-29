using Filian.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class ThemesViewModel : ViewModel
    {
        public ObservableCollection<Theme> Themes { get; set; }

        private static Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                OnPropertyChanged("SelectedTheme");
            }
        }

        public ThemesViewModel()
        {
            string sqlForTheme = 
                $"SELECT themes.id, name, picture_path, translation FROM themes LEFT JOIN themes_translations ON themes.id = themes_translations.theme_id WHERE themes_translations.language_id = {MainViewModel.LanguageId} AND  themes.id <= 20;";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                Log.Info("Successfully connected to database.");

                SqlCommand sqlCommand = new SqlCommand(sqlForTheme, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                Themes = new ObservableCollection<Theme>();
                while (sqlDataReader.Read())
                {
                    Themes.Add(new Theme
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2),
                        Translation = sqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected themes info from database.");

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select themes info from database: ", ex);
            }
        }
    }
}