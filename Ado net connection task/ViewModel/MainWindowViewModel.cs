using Ado_net_connection_task.Commands;
using Ado_net_connection_task.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
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

        private int id;

        public int Id
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

        private string selectedItem;

        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }


        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; OnPropertyChanged(); }
        }


        public RelayCommand Insert { get; set; }
        public RelayCommand Delete { get; set; }
        public RelayCommand SelectionChanged { get; set; }

        public bool Clear { get; set; } = true;

        public ObservableCollection<Author> ListAuthors { get; set; }

        public void Connection(ObservableCollection<Author> authors)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn.Open();


                var query = "SELECT * FROM Authors";

                SqlDataReader reader = null;

                using (var command = new SqlCommand(query, conn))
                {
                    authors.Clear();

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Author author = new Author();

                        author.Id = (int)reader[0];
                        author.FirstName = reader[1].ToString();
                        author.LastName = reader[2].ToString();
                        authors.Add(author);

                    }
                    ListAuthors = authors;
                }
            }
        }

        public MainWindowViewModel()
        {
            ObservableCollection<Author> authors = new ObservableCollection<Author>();

            Connection(authors);
            SelectionChanged = new RelayCommand((a) =>
            {
                if (SelectedIndex != -1)
                {
                    using (var conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                        conn.Open();

                        using (var command = new SqlCommand("sp_AuthorsDelete", conn))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var id = new SqlParameter();
                            id.ParameterName = "@id";
                            id.SqlDbType = SqlDbType.Int;
                            id.Value = authors[selectedIndex].Id - 1;

                            command.Parameters.Add(id);

                            var result = command.ExecuteNonQuery();
                            MessageBox.Show($"Deleted successfully.");
                        }

                        Connection(authors);

                    }
                }
            });

            Insert = new RelayCommand((a) =>
            {
                using (var conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                    conn.Open();

                    using (var command = new SqlCommand("sp_AuthorInsert", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var paramId = new SqlParameter();
                        paramId.ParameterName = "@id";
                        paramId.SqlDbType = SqlDbType.Int;
                        paramId.Value = Id;

                        var paramName = new SqlParameter();
                        paramName.ParameterName = "@firstName";
                        paramName.SqlDbType = SqlDbType.NVarChar;
                        paramName.Value = Firstname;

                        var paramSurname = new SqlParameter();
                        paramSurname.ParameterName = "@lastName";
                        paramSurname.SqlDbType = SqlDbType.NVarChar;
                        paramSurname.Value = Lastname;

                        command.Parameters.Add(paramId);
                        command.Parameters.Add(paramName);
                        command.Parameters.Add(paramSurname);

                        bool equalId = false;

                        for (int i = 0; i < authors.Count; i++)
                        {
                            if (authors[i].Id == Id)
                            {
                                MessageBox.Show($"It was unsuccessful to add");
                                equalId = true;
                                break;
                            }
                        }
                        if (!equalId)
                        {
                            var result = command.ExecuteNonQuery();
                            MessageBox.Show($"Added successfully.");
                        }
                    }

                    Connection(authors);

                    Id = 00;
                    Firstname = String.Empty;
                    Lastname = String.Empty;
                }
            });
        }
    }
}
