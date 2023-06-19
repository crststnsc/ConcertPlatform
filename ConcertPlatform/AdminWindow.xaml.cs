using ConcertPlatform.Models.DataAccessLayer;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ArtistsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Artist> artists = GetArtistsFromDatabase();
            dataGrid.ItemsSource = artists;
        }

        private void VenuesButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Venue> venues = GetVenuesFromDatabase();
            dataGrid.ItemsSource = venues;
        }

        private void ConcertsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Concert> concerts = GetConcertsFromDatabase();
            dataGrid.ItemsSource = concerts;
        }

        private ObservableCollection<Artist> GetArtistsFromDatabase()
        {
            ObservableCollection<Artist> artists = new ();

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
                                ArtistId = reader.GetInt32(0),
                                ArtistName = reader.GetString(1),
                                Genre = reader.GetString(2),
                                Bio = reader.GetString(3),
                                ContactInformation = reader.GetString(4)
                            };

                            artists.Add(artist);
                        }
                    }
                }
            }

            return artists;
        }
        private ObservableCollection<Venue> GetVenuesFromDatabase()
        {
            ObservableCollection<Venue> venues = new ();

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
                                VenueId = reader.GetInt32(0),
                                VenueName = reader.GetString(1),
                                Location = reader.GetString(2),
                                Capacity = reader.GetInt32(3),
                                ContactInformation = reader.GetString(4)
                            };

                            venues.Add(venue);
                        }
                    }
                }
            }

            return venues;
        }
        private ObservableCollection<Concert> GetConcertsFromDatabase()
        {
            ObservableCollection<Concert> concerts = new ();

            using (var connection = DALHelper.Connection)
            {
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
                                ConcertId = reader.GetInt32(0),
                                ArtistId = reader.GetInt32(1),
                                VenueId = reader.GetInt32(2),
                                Date = DateOnly.FromDateTime(reader.GetDateTime(3)),
                                Time = TimeOnly.FromTimeSpan(reader.GetTimeSpan(4)),
                                TicketPrice = reader.GetDecimal(5),
                                Description = reader.GetString(6),
                            };

                            concerts.Add(concert);
                        }
                    }
                }
            }

            return concerts;
        }
        
    }
}
