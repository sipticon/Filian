using Filian.Core;
using Filian.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class ThemesViewModel : ObservableObject
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
            string sqlForTheme = " SELECT * FROM themes";
            string ssqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(ssqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlForTheme, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            Themes = new ObservableCollection<Theme>();
            while (sqlDataReader.Read())
            {
                if(sqlDataReader.GetInt32(0) <= 20)
                    Themes.Add(new Theme
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