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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {

        // In your AdminWindow class

        // Initialize the ComboBoxes for artists and venues
        private void InitializeComboBoxes()
        {
            // Artists ComboBox
            ObservableCollection<Artist> artists = AdminWindow.GetArtistsFromDatabase();
            artistComboBox.ItemsSource = artists;
            artistComboBox.DisplayMemberPath = "Artist_Name";

            // Venues ComboBox
            ObservableCollection<Venue> venues = AdminWindow.GetVenuesFromDatabase();
            venueComboBox.ItemsSource = venues;
            venueComboBox.DisplayMemberPath = "Venue_Name";
        }

        private void InitializeTimeComboBoxes()
        {
            // Hours ComboBox
            for (int hour = 0; hour < 24; hour++)
            {
                hourComboBox.Items.Add(hour.ToString("00"));
            }

            // Minutes ComboBox
            for (int minute = 0; minute < 60; minute += 15) // You can adjust the step as needed
            {
                minuteComboBox.Items.Add(minute.ToString("00"));
            }
        }

        public AddWindow()
        {
            InitializeComponent();
            InitializeComboBoxes();
            InitializeTimeComboBoxes();
        }

        private void AddArtistButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from input fields
            string artistName = artistNameTextBox.Text;
            string genre = genreTextBox.Text;
            string bio = bioTextBox.Text;
            string contactInfo = contactInfoTextBox.Text;

            // Insert data into database
            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string insertQuery = "INSERT INTO Artists (artist_name, genre, bio, contact_information) " +
                                     "VALUES (@ArtistName, @Genre, @Bio, @ContactInfo)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("ArtistName", artistName);
                    command.Parameters.AddWithValue("Genre", genre);
                    command.Parameters.AddWithValue("Bio", bio);
                    command.Parameters.AddWithValue("ContactInfo", contactInfo);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Artist added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add artist.");
                    }
                }
            }
        }

        private void AddVenueButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from input fields
            string venueName = venueNameTextBox.Text;
            string location = locationTextBox.Text;
            int capacity = int.Parse(capacityTextBox.Text);
            string contactInfo = contactInfoTextBox.Text;

            // Insert data into database
            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string insertQuery = "INSERT INTO Venues (venue_name, location, capacity, contact_information) " +
                                     "VALUES (@VenueName, @Location, @Capacity, @ContactInfo)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("VenueName", venueName);
                    command.Parameters.AddWithValue("Location", location);
                    command.Parameters.AddWithValue("Capacity", capacity);
                    command.Parameters.AddWithValue("ContactInfo", contactInfo);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Venue added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add venue.");
                    }
                }
            }
        }

        private void AddConcertButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from input fields
            int artistId = ((Artist)artistComboBox.SelectedItem).Artist_Id;
            int venueId = ((Venue)venueComboBox.SelectedItem).Venue_Id;
            DateTime selectedDateTime = datePicker.SelectedDate.Value.Date + 
                new TimeSpan(int.Parse(hourComboBox.SelectedItem.ToString()), int.Parse(minuteComboBox.SelectedItem.ToString()), 0);
            decimal ticketPrice = decimal.Parse(ticketPriceTextBox.Text);
            string description = descriptionTextBox.Text;

            // Insert data into database
            using (var connection = DALHelper.Connection)
            {
                connection.Open();

                string insertQuery = "INSERT INTO Concerts (artist_id, venue_id, date, time, ticket_price, description) " +
                                     "VALUES (@ArtistId, @VenueId, @Date, @Time, @TicketPrice, @Description)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("ArtistId", artistId);
                    command.Parameters.AddWithValue("VenueId", venueId);
                    command.Parameters.AddWithValue("Date", selectedDateTime.Date);
                    command.Parameters.AddWithValue("Time", selectedDateTime.TimeOfDay);
                    command.Parameters.AddWithValue("TicketPrice", ticketPrice);
                    command.Parameters.AddWithValue("Description", description);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Concert added successfully!");

                    }
                    else
                    {
                        MessageBox.Show("Failed to add concert.");
                    }
                }
            }
        }

    }
}
