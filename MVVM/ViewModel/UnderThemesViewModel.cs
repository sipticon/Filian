using Filian.Core;
using Filian.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class UnderThemesViewModel : ObservableObject
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public ObservableCollection<Theme> UnderThemes { get; set; }

        private static ObservableCollection<Theme> _selectedItems;

        public ObservableCollection<Theme> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                OnPropertyChanged("SelectedItems");
            }
        }

        public UnderThemesViewModel()
        {
            MainViewModel mainViewModel = new MainViewModel();
            SelectedItems = new ObservableCollection<Theme>();

            string sqlForUnderTheme = 
                $"SELECT themes.id, name, picture_path, translation FROM themes LEFT JOIN themes_translations ON themes.id = themes_translations.theme_id WHERE themes.parenttheme_id = {mainViewModel.ThemeId} AND themes_translations.language_id = {MainViewModel.LanguageId};";
            string sqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                Log.Info("Successfully connected to database.");

                SqlCommand sqlCommand = new SqlCommand(sqlForUnderTheme, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                UnderThemes = new ObservableCollection<Theme>();
                while (sqlDataReader.Read())
                {
                    int currentId = sqlDataReader.GetInt32(0);
                    UnderThemes.Add(new Theme
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Name = sqlDataReader.GetString(1),
                        PicturePath = sqlDataReader.GetString(2),
                        Translation = sqlDataReader.GetString(3)
                    });
                }

                Log.Info("Successfully selected underThemes info from database.");

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Failed while trying to select underThemes info from database: ", ex);
            }
        }
    }
}