using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class UnderThemesViewModel : ObservableObject
    {
        public ObservableCollection<UnderTheme> UnderThemes { get; set; }

        private static ObservableCollection<UnderTheme> _selectedItems;

        public ObservableCollection<UnderTheme> SelectedItems
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
            SelectedItems = new ObservableCollection<UnderTheme>();

            string sqlForUnderTheme = $" SELECT * FROM themes WHERE parenttheme_id = {mainViewModel.ThemeId}";
            string ssqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(ssqlConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlForUnderTheme, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            UnderThemes = new ObservableCollection<UnderTheme>();
            while (sqlDataReader.Read())
            {
                UnderThemes.Add(new UnderTheme
                {
                    Id = sqlDataReader.GetInt32(0), 
                    Name = sqlDataReader.GetString(1), 
                    PicturePath = sqlDataReader.GetString(2)
                });
            }
            sqlConnection.Close();
        }
    }
}