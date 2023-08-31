using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    internal enum Table
    {
        Artists,
        Venues,
        Concerts
    }

    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private Table table;

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ArtistsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Artist> artists = GetArtistsFromDatabase();
            dataGrid.ItemsSource = artists;
            table = Table.Artists;
        }

        private void VenuesButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Venue> venues = GetVenuesFromDatabase();
            dataGrid.ItemsSource = venues;
            table = Table.Venues;
        }

        private void ConcertsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Concert> concerts = GetConcertsFromDatabase();
            dataGrid.ItemsSource = concerts;
            table = Table.Concerts;
        }

        private void PurchasedTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Purchase> purchasedTickets = GetPurchasedTicketsFromDatabase();
            dataGrid.ItemsSource = purchasedTickets;
        }


        public static ObservableCollection<Artist> GetArtistsFromDatabase()
        {
            ObservableCollection<Artist> artists = new();

            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string query = "SELECT * FROM Artists";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artist artist = new Artist
                            {
                                Artist_Id = reader.GetInt32(0),
                                Artist_Name = reader.GetString(1),
                                Genre = reader.GetString(2),
                                Bio = reader.GetString(3),
                                Contact_Information = reader.GetString(4)
                            };

                            artists.Add(artist);
                        }
                    }
                }
            }

            return artists;
        }
        public static ObservableCollection<Venue> GetVenuesFromDatabase()
        {
            ObservableCollection<Venue> venues = new();

            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string query = "SELECT * FROM Venues";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Venue venue = new Venue
                            {
                                Venue_Id = reader.GetInt32(0),
                                Venue_Name = reader.GetString(1),
                                Location = reader.GetString(2),
                                Capacity = reader.GetInt32(3),
                                Contact_Information = reader.GetString(4)
                            };

                            venues.Add(venue);
                        }
                    }
                }
            }

            return venues;
        }
        public static ObservableCollection<Concert> GetConcertsFromDatabase()
        {
            ObservableCollection<Concert> concerts = new();

            using var connection = DALHelper.Connection;
            connection.Open();

            string query = "SELECT * FROM Concerts";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Concert concert = new Concert
                        {
                            Concert_Id = reader.GetInt32(0),
                            Artist_Id = reader.GetInt32(1),
                            Venue_Id = reader.GetInt32(2),
                            Date = DateOnly.FromDateTime(reader.GetDateTime(3)),
                            Time = TimeOnly.FromTimeSpan(reader.GetTimeSpan(4)),
                            Ticket_Price = reader.GetDecimal(5),
                            Description = reader.GetString(6),
                        };

                        concerts.Add(concert);
                    }
                }
            }


            return concerts;
        }
        private ObservableCollection<Purchase> GetPurchasedTicketsFromDatabase()
        {
            ObservableCollection<Purchase> purchasedTickets = new();

            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string query = "SELECT * FROM purchases";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Purchase purchasedTicket = new()
                            {
                                UserId = reader.GetInt32(0),
                                TicketId = reader.GetInt32(1),
                            };

                            purchasedTickets.Add(purchasedTicket);
                        }
                    }
                }
            }

            return purchasedTickets;
        }

        private void DeleteRecordFromDatabase(int id, string tableName, string primaryKeyColumn)
        {
            using var connection = DALHelper.Connection;
            connection.Open();

            string query = $"DELETE FROM {tableName} WHERE {primaryKeyColumn} = @Id";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("Id", id);

            var rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                MessageBox.Show("Error deleting record.");
                return;
            }
            MessageBox.Show("Record deleted successfully.");
        }

        private void RefreshDataGrid()
        {
            switch (table)
            {
                case Table.Artists:
                    ArtistsButton_Click(null, null);
                    break;
                case Table.Venues:
                    VenuesButton_Click(null, null);
                    break;
                case Table.Concerts:
                    ConcertsButton_Click(null, null);
                    break;
                    // Handle other cases if needed
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            switch (table)
            {
                case Table.Artists:
                    DeleteRecordFromDatabase((selectedItem as Artist).Artist_Id, "Artists", "artist_id");
                    break;
                case Table.Venues:
                    DeleteRecordFromDatabase((selectedItem as Venue).Venue_Id, "Venues", "venue_id");
                    break;
                case Table.Concerts:
                    DeleteRecordFromDatabase((selectedItem as Concert).Concert_Id, "Concerts", "concert_id");
                    break;
                    // Handle other cases if needed
            }

            // Refresh the data grid
            RefreshDataGrid();
        }

        private void UpdateTable_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem;

            switch (table)
            {
                case Table.Artists:
                    UpdateRecordInDatabase<Artist>(selectedItem as Artist, "Artists", "artist_id");
                    break;
                case Table.Venues:
                    UpdateRecordInDatabase<Venue>(selectedItem as Venue, "Venues", "venue_id");
                    break;
                case Table.Concerts:
                    UpdateRecordInDatabase<Concert>(selectedItem as Concert, "Concerts", "concert_id");
                    break;
            }
        }

        private void UpdateRecordInDatabase<T>(T record, string tableName, string primaryKeyColumn)
        {
            using var connection = DALHelper.Connection;
            connection.Open();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var updateColumns = properties.Select(p => p.Name + " = @" + p.Name);

            string query = $"UPDATE {tableName} SET {string.Join(", ", updateColumns)} WHERE {primaryKeyColumn} = @{primaryKeyColumn}";

            using (var command = new NpgsqlCommand(query, connection))
            {
                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue(property.Name, property.GetValue(record));
                }

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    MessageBox.Show("Error updating table");
                    return;
                }
                MessageBox.Show("Updated successfully");
            }
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addRecordWindow = new();
            addRecordWindow.ShowDialog();
        }

    }
}
