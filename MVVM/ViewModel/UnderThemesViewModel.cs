using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class UnderThemesViewModel : ObservableObject
    {
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

            string sqlForUnderTheme = $" SELECT * FROM themes WHERE parenttheme_id = {mainViewModel.ThemeId}";
            string ssqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(ssqlConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlForUnderTheme, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            UnderThemes = new ObservableCollection<Theme>();
            while (sqlDataReader.Read())
            {
                UnderThemes.Add(new Theme
                {
                    Id = sqlDataReader.GetInt32(0), 
                    Name = sqlDataReader.GetString(1), 
                    Picture_Path = sqlDataReader.GetString(2)
                });
            }
            sqlConnection.Close();
        }
    }
}