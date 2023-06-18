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
        public BasketItem BasketItem { get; set; }

        public NameSeatWindow(BasketItem basketItem)
        {
            InitializeComponent();
            BasketItem = basketItem;
            name.Text = BasketItem.TicketHolderName;
            seat.Text = BasketItem.SeatNumber.ToString();
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
            command.Parameters.AddWithValue("TicketId", BasketItem.TicketId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Seat number and ticket holder name updated successfully for ticket ID: " + BasketItem.TicketId);
            }
            else
            {
                MessageBox.Show("Ticket ID not found or no rows updated.");
            }
        }
    }
}

