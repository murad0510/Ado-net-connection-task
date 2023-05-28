using Ado_net_connection_task.Commands;
using Ado_net_connection_task.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ado_net_connection_task.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }


        private string firstname;

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; OnPropertyChanged(); }
        }

        private string lastname;

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; OnPropertyChanged(); }
        }

        public RelayCommand Insert { get; set; }
        public RelayCommand Delete { get; set; }

        public ObservableCollection<Author> ListAuthors { get; set; }

        public MainWindowViewModel()
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn.Open();

                List<Author> author = new List<Author>();

                SqlDataReader reader = null;

                var id = new SqlParameter();
                id.SqlDbType = SqlDbType.Int OUT;
                id.ParameterName = "@id";

                var firstname = new SqlParameter();
                firstname.SqlDbType = SqlDbType.NVarChar;
                firstname.ParameterName = "@firstname";

                var lastname = new SqlParameter();
                lastname.SqlDbType = SqlDbType.NVarChar;
                lastname.ParameterName = "@lastname";


                using (var command = new SqlCommand("sp_TakeById", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        command.Parameters.Add(id);
                        command.Parameters.Add(firstname);
                        command.Parameters.Add(lastname);


                    }

                }

            }
        }
    }
}
