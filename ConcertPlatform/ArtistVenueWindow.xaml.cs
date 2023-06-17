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
    /// Interaction logic for ArtistVenueWindow.xaml
    /// </summary>
    public partial class ArtistVenueWindow : Window
    {
        public Artist artist;
        public Venue venue;
        public ArtistVenueWindow(int a, int v)
        {
            InitializeComponent();
            DataContext = this;
            LoadArtist(a);
            LoadVenue(v);
            artist_txt.Text = artist.ToString();
            venue_txt.Text = venue.ToString();
        }

        void LoadArtist(int a)
        {
            //open connection
            var connection = DALHelper.Connection;
            connection.Open();

            string query = "SELECT * FROM ARTISTS WHERE artist_id = @ArtistId";
            var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("ArtistId", a);

            NpgsqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                artist = new Artist()
                {
                    ArtistId = reader.GetInt32(0),
                    ArtistName = reader.GetString(1),
                    Genre = reader.GetString(2),
                    Bio = reader.GetString(3),
                    ContactInformation = reader.GetString(4)
                };
            }

            connection.Close();
        }

        void LoadVenue(int v)
        {
            var connection = DALHelper.Connection;
            connection.Open();

            string query = "SELECT * FROM VENUES WHERE venue_id = @VenueId";

            var cmd = new NpgsqlCommand(query, connection);

            cmd.Parameters.AddWithValue("VenueId", v);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();

                venue = new Venue()
                {
                    VenueId = reader.GetInt32(0),
                    VenueName = reader.GetString(1),
                    Location = reader.GetString(2),
                    Capacity = reader.GetInt32(3),
                    ContactInformation = reader.GetString(4)
                };
            }

            connection.Close();
        }
    }
}
