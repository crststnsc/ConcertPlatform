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
      
        int user_id;

        public MenuWindow(int user_id)
        {
            InitializeComponent();
            DataContext = this;
            LoadConcerts();
            this.user_id = user_id;
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
                DataTable dataTable = new DataTable();
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
                string getMaxSeatNumberQuery = "SELECT MAX(seat_number) FROM Tickets WHERE concert_id = @ConcertId";
                using (var getMaxSeatNumberCommand = new NpgsqlCommand(getMaxSeatNumberQuery, connection))
                {
                    getMaxSeatNumberCommand.Parameters.AddWithValue("ConcertId", concert.ConcertId);
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
                    string getVenueCapacityQuery = "SELECT capacity FROM Venues WHERE venue_id = @VenueId";
                    using (var getVenueCapacityCommand = new NpgsqlCommand(getVenueCapacityQuery, connection))
                    {
                        getVenueCapacityCommand.Parameters.AddWithValue("VenueId", concert.VenueId);
                        int venueCapacity = (int)getVenueCapacityCommand.ExecuteScalar();

                        // Check if the seat number exceeds the venue capacity
                        if (nextSeatNumber > venueCapacity)
                        {
                            MessageBox.Show("Seat number exceeds venue capacity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return -1;
                        }

                        // Insert the new ticket row
                        string insertTicketQuery = 
                            "INSERT INTO Tickets (concert_id, seat_number) VALUES (@ConcertId, @SeatNumber) RETURNING ticket_id";
                        using (var insertTicketCommand = new NpgsqlCommand(insertTicketQuery, connection))
                        {
                            insertTicketCommand.Parameters.AddWithValue("ConcertId", concert.ConcertId);
                            insertTicketCommand.Parameters.AddWithValue("SeatNumber", nextSeatNumber);
                            
                            ticketId = insertTicketCommand.ExecuteScalar() as int? ?? -1;
                        }
                    }
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
                ConcertId = (int)selectedRow["concert_id"],
                ArtistId = (int)selectedRow["artist_id"],
                VenueId = (int)selectedRow["venue_id"],
                Date = DateOnly.FromDateTime((DateTime)selectedRow["date"]),
                Time = TimeOnly.FromTimeSpan((TimeSpan)selectedRow["time"]),
                TicketPrice = (decimal)selectedRow["ticket_price"],
                Description = (string)selectedRow["description"],
            };

            int ticket_id = GenerateTicket(selectedConcert);

            var connection = DALHelper.Connection;
            connection.Open();

            string query = "INSERT INTO Basket (user_id, ticket_id) VALUES (@user_id, @ticket_id)";

            var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("user_id", user_id);
            cmd.Parameters.AddWithValue("ticket_id", ticket_id);
            cmd.ExecuteNonQuery();           
        }

        private void ViewBasket_Click(object sender, RoutedEventArgs e)
        {
            ShoppingBasketWindow shoppingBasketWindow = new(user_id);
            shoppingBasketWindow.Show();
        }   
    }
}

