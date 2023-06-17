using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ConcertPlatform
{
    /// <summary>
    /// Interaction logic for NameSeatWindow.xaml
    /// </summary>
    public partial class NameSeatWindow : Window
    {
        int ticket_id;

        public NameSeatWindow(int ticket_id)
        {
            InitializeComponent();
            this.ticket_id = ticket_id;
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UpdateTicket_Click(object sender, RoutedEventArgs e)
        {
            if(name.Text == "" || seat.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var connection = DALHelper.Connection;
            connection.Open();

            string updateQuery = "UPDATE Tickets SET seat_number = @NewSeatNumber, ticket_holder_name = @NewTicketHolderName WHERE ticket_id = @TicketId";

            var command = new NpgsqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("NewSeatNumber", seat.Text);
            command.Parameters.AddWithValue("NewTicketHolderName", name.Text);
            command.Parameters.AddWithValue("TicketId", ticket_id);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Seat number and ticket holder name updated successfully for ticket ID: " + ticket_id);
            }
            else
            {
                MessageBox.Show("Ticket ID not found or no rows updated.");
            }
        }
    }
}

