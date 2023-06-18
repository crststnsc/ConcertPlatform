using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
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
    /// Interaction logic for AccountSettingsWindow.xaml
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        public User User { get; set; }

        public AccountSettingsWindow(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = this;
        }

        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if(name.Text == "" || password.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var connection = DALHelper.Connection;
            connection.Open();

            string updateQuery = "UPDATE Users SET username = @NewUsername, password = @NewPassword WHERE user_id = @UserId";

            var command = new NpgsqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("NewUsername", name.Text);
            command.Parameters.AddWithValue("password", password.Text);
            command.Parameters.AddWithValue("UserId", User.UserId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Username or password successfully update for user ID: " + User.UserId);
            }
            else
            {
                MessageBox.Show("User ID not found or no rows updated.");
            }
        }
    }
}
