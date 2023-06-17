using ConcertPlatform.Models.DataAccessLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConcertPlatform
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void register_btn_Click(object sender, RoutedEventArgs e){
            NpgsqlConnection conn = DALHelper.Connection;
            conn.Open();

            string query = "SELECT * FROM Users WHERE username = @username";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@username", username_box.Text);

            NpgsqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                MessageBox.Show("Username already taken!");
                return;
            }

            if (password_box.Password != confirm_box.Password)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }
            reader.Close();

            var register_query = "INSERT INTO Users(username, password, isAdmin) VALUES (@username, @password, false)";            
            var register_command = new NpgsqlCommand(register_query, conn);
            register_command.Parameters.AddWithValue("@username", username_box.Text);
            register_command.Parameters.AddWithValue("@password", password_box.Password);

            int i = register_command.ExecuteNonQuery();

            if (i == -1)
            {
                MessageBox.Show("Error trying to register. Please try again later");
            }
            else
            {
                MessageBox.Show("You can now login!");
            }

            conn.Close();
        }
    }
}
