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
    /// Interaction logic for ShoppingBasketWindow.xaml
    /// </summary>
    public partial class ShoppingBasketWindow : Window
    {
        private int user_id;

        public ShoppingBasketWindow(int user_id)
        {
            InitializeComponent();
            DataContext = this;
            this.user_id = user_id;
            LoadBasket(user_id);
        }

        void LoadBasket(int user_id)
        {
            using var connection = DALHelper.Connection;
            connection.Open();

            string query = "SELECT t.ticket_id, c.concert_id, c.date, c.time, c.ticket_price, a.artist_name " +
                           "FROM Tickets t " +
                           "INNER JOIN Concerts c ON t.concert_id = c.concert_id " +
                           "LEFT JOIN Artists a ON c.artist_id = a.artist_id " +
                           "INNER JOIN Basket b ON t.ticket_id = b.ticket_id " +
                           "WHERE b.user_id = @UserId";


            NpgsqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("UserId", user_id);

            NpgsqlDataAdapter adapter = new(cmd);
            DataTable dataTable = new();
            adapter.Fill(dataTable);

            ObservableCollection<BasketItem> basketItems = new ObservableCollection<BasketItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                Debug.WriteLine("This is basket:" + row["concert_id"]);

                BasketItem basketItem = new BasketItem {
                    TicketId = Convert.ToInt32(row["ticket_id"]),
                    ConcertId = Convert.ToInt32(row["concert_id"]),
                    Date = Convert.ToDateTime(row["date"]),
                    Time = TimeSpan.Parse(row["time"].ToString()),
                    TicketPrice = Convert.ToDecimal(row["ticket_price"]),
                    ArtistName = row["artist_name"].ToString()
                };
                basketItems.Add(basketItem);
            }

            basketListBox.ItemsSource = basketItems;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            BasketItem basketItem = (BasketItem)basketListBox.SelectedItem;
            using var connection = DALHelper.Connection;
            connection.Open();

            string query = "DELETE FROM Basket WHERE ticket_id = @TicketId AND user_id = @UserId";

            NpgsqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("TicketId", basketItem.TicketId);
            cmd.Parameters.AddWithValue("UserId", user_id);

            cmd.ExecuteNonQuery();

            LoadBasket(user_id);
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            BasketItem basketItem = (BasketItem)basketListBox.SelectedItem;
            using var connection = DALHelper.Connection;
            connection.Open();

            string query = "DELETE FROM Basket WHERE ticket_id = @TicketId AND user_id = @UserId";

            NpgsqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("TicketId", basketItem.TicketId);
            cmd.Parameters.AddWithValue("UserId", user_id);

            cmd.ExecuteNonQuery();

            query = "INSERT INTO Purchases (user_id, ticket_id) VALUES (@UserId, @TicketId)";

            cmd = new(query, connection);
            cmd.Parameters.AddWithValue("TicketId", basketItem.TicketId);
            cmd.Parameters.AddWithValue("UserId", user_id);

            cmd.ExecuteNonQuery();

            LoadBasket(user_id);
        }

        private void RemoveAll_Click(object sender, RoutedEventArgs e)
        {
            using var connection = DALHelper.Connection;
            connection.Open();

            string query = "DELETE FROM Basket WHERE user_id = @UserId";

            NpgsqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("UserId", user_id);

            cmd.ExecuteNonQuery();

            LoadBasket(user_id);
        }
    }
}
