using Filian.Core;
using System.Collections.ObjectModel;
using Filian.MVVM.Model;
using System.Data.SqlClient;

namespace Filian.MVVM.ViewModel
{
    public class TestsViewModel : ObservableObject
    {
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
            string sqlForTest = " SELECT * FROM tests";
            string ssqlConnectionString =
                @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

            SqlConnection sqlConnection = new SqlConnection(ssqlConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlForTest, sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Tests = new ObservableCollection<Test>();
            while (sqlDataReader.Read())
            {
               Tests.Add(new Test
               {
                   Id = sqlDataReader.GetInt32(0), 
                   ImagePath = sqlDataReader.GetString(2), 
                   Name = sqlDataReader.GetString(1)
               }); 
            }
            sqlConnection.Close();
        }
    }
}