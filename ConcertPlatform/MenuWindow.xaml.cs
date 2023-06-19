using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
      
        private User user;

        public MenuWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoadConcerts();
            this.user = user;
        }

        private void LoadConcerts()
        {

            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string query = "SELECT c.concert_id, c.date, c.time, c.ticket_price, c.description, c.artist_id, a.artist_name,c.venue_id, v.venue_name " +
                               "FROM Concerts c " +
                               "LEFT JOIN Artists a ON c.artist_id = a.artist_id " +
                               "LEFT JOIN Venues v ON c.venue_id = v.venue_id";

                NpgsqlCommand cmd = new(query, connection);

                NpgsqlDataAdapter adapter = new(cmd);
                DataTable dataTable = new();
                adapter.Fill(dataTable);
                concertsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        int GenerateTicket(Concert concert)
        {
            int ticketId;

            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                // Retrieve the maximum seat number from the referenced venue
                string getMaxSeatNumberQuery = "SELECT MAX(seat_number) FROM Tickets WHERE concert_id = @Concert_Id";
                using (var getMaxSeatNumberCommand = new NpgsqlCommand(getMaxSeatNumberQuery, connection))
                {
                    getMaxSeatNumberCommand.Parameters.AddWithValue("Concert_Id", concert.Concert_Id);
                    object maxSeatNumberObj = getMaxSeatNumberCommand.ExecuteScalar();
                    Debug.WriteLine("max seat number :" + maxSeatNumberObj);

                    int? maxSeatNumber = null;
                    if (maxSeatNumberObj != DBNull.Value && int.TryParse(maxSeatNumberObj.ToString(), out int parsedSeatNumber))
                    {
                        maxSeatNumber = parsedSeatNumber;
                    }


                    // Determine the next available seat number
                    int nextSeatNumber = maxSeatNumber.HasValue ? maxSeatNumber.Value + 1 : 1;

                    // Retrieve the capacity of the venue
                    string getVenueCapacityQuery = "SELECT capacity FROM Venues WHERE venue_id = @Venue_Id";
                    var getVenueCapacityCommand = new NpgsqlCommand(getVenueCapacityQuery, connection);
                    getVenueCapacityCommand.Parameters.AddWithValue("Venue_Id", concert.Venue_Id);
                    int venueCapacity = (int)getVenueCapacityCommand.ExecuteScalar();

                    // Check if the seat number exceeds the venue capacity
                    if (nextSeatNumber > venueCapacity)
                    {
                        MessageBox.Show("Seat number exceeds venue capacity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return -1;
                    }

                    // Insert the new ticket row
                    string insertTicketQuery =
                        "INSERT INTO Tickets (concert_id, seat_number, ticket_holder_name, purchase_date, status)" +
                        "VALUES (@Concert_Id, @SeatNumber, @TicketHolder, @PurchaseDate, @Status)" +
                        "RETURNING ticket_id";
                    var insertTicketCommand = new NpgsqlCommand(insertTicketQuery, connection);
                    insertTicketCommand.Parameters.AddWithValue("Concert_Id", concert.Concert_Id);
                    insertTicketCommand.Parameters.AddWithValue("SeatNumber", nextSeatNumber);
                    insertTicketCommand.Parameters.AddWithValue("TicketHolder", user.Username);
                    insertTicketCommand.Parameters.AddWithValue("PurchaseDate", DateOnly.FromDateTime(DateTime.Now));
                    insertTicketCommand.Parameters.AddWithValue("Status", "pending");

                    ticketId = insertTicketCommand.ExecuteScalar() as int? ?? -1;
                }
            }   

            Debug.WriteLine("Generated ticket id is: " + ticketId);
            return ticketId;
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = concertsDataGrid.SelectedItem as DataRowView;

            Debug.WriteLine(selectedRow["concert_id"]);

            if (selectedRow == null)
            {
                MessageBox.Show("Please select a concert first!");
                return;
            }


            Concert selectedConcert = new Concert
            {
                Concert_Id = (int)selectedRow["concert_id"],
                Artist_Id = (int)selectedRow["artist_id"],
                Venue_Id = (int)selectedRow["venue_id"],
                Date = DateOnly.FromDateTime((DateTime)selectedRow["date"]),
                Time = TimeOnly.FromTimeSpan((TimeSpan)selectedRow["time"]),
                Ticket_Price = (decimal)selectedRow["ticket_price"],
                Description = (string)selectedRow["description"],
            };

            int ticket_id = GenerateTicket(selectedConcert);

            var connection = DALHelper.Connection;
            connection.Open();

            string query = "INSERT INTO Basket (user_id, ticket_id) VALUES (@user_id, @ticket_id)";

            var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("user_id", user.UserId);
            cmd.Parameters.AddWithValue("ticket_id", ticket_id);
            cmd.ExecuteNonQuery();           
        }

        private void ViewBasket_Click(object sender, RoutedEventArgs e)
        {
            ShoppingBasketWindow shoppingBasketWindow = new(user.UserId);
            shoppingBasketWindow.Show();
        }   

        private void ViewArtist_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = concertsDataGrid.SelectedItem as DataRowView;

            if(selectedRow == null)
            {
                MessageBox.Show("Please select a concert first!");
                return;
            }

            int artistId = (int)selectedRow["artist_id"];
            int venueId = (int)selectedRow["venue_id"];

            ArtistVenueWindow artistVenueWindow = new(artistId, venueId);
            artistVenueWindow.Show();
        }

        private void AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            AccountSettingsWindow accountSettingsWindow = new(user);
            accountSettingsWindow.Show();
        }
    }
}

