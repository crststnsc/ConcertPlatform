using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConcertPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = DALHelper.Connection;
            conn.Open();

            string query = "SELECT * FROM Users WHERE username = @username AND password = @password";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@username", username_txt.Text);
            command.Parameters.AddWithValue("@password", password_txt.Password);

            NpgsqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                User user = new()
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Isadmin = reader.GetBoolean(3)
                };
                
                if(user.Isadmin)
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else
                {
                    MenuWindow menuWindow = new(user);
                    menuWindow.Show();
                }   
                Close();
            }
            else
            {
                //message that the username or password is incorrect
                MessageBox.Show("Incorrect username or password");
            }

            conn.Close();
        }

        private void register_btn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new();
            register.Show();
        }
    }
}
