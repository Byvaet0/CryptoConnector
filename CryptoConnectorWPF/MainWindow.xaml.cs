using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CryptoConnector.Endpoints;
using CryptoConnector.Models;
using CryptoConnector.MyBalance;
using WebSocketSharp;

namespace CryptoConnectorWPF
{
    public partial class MainWindow : Window
    {
        private WebSocket _ws;
        public ObservableCollection<string> Messages { get; set; }
        public ObservableCollection<Balance> Balances { get; set; } 
        private MainBalance _mainBalance;

        public MainWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<string>();
            Balances = new ObservableCollection<Balance>(); 
            DataContext = this;
            _mainBalance = new MainBalance();
        }

        private void ConnectWebSocket(string pair, string dataType)
        {
            string symbol = $"t{pair.ToUpper()}";

            _ws = new WebSocket("wss://api-pub.bitfinex.com/ws/2");

            _ws.OnMessage += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add($"Получено сообщение: {e.Data}");
                });
            };

            _ws.Connect();

            if (dataType == "Trades")
            {
                _ws.Send($"{{\"event\":\"subscribe\",\"channel\":\"trades\",\"symbol\":\"{symbol}\"}}");
            }
            else if (dataType == "Candles")
            {
                _ws.Send($"{{\"event\":\"subscribe\",\"channel\":\"candles\",\"symbol\":\"{symbol}\",\"key\":\"1m\"}}");
            }
        }

        private void DisconnectWebSocket()
        {
            if (_ws != null && _ws.IsAlive)
            {
                _ws.Close();
                Messages.Add("Соединение закрыто.");
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string pair = PairTextBox.Text.Trim();
            if (string.IsNullOrEmpty(pair))
            {
                Messages.Add("Введите валютную пару.");
                return;
            }

            string dataType = ((ComboBoxItem)DataTypeComboBox.SelectedItem)?.Content.ToString();
            if (string.IsNullOrEmpty(dataType))
            {
                Messages.Add("Выберите тип данных.");
                return;
            }

            Messages.Clear();
            ConnectWebSocket(pair, dataType);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            DisconnectWebSocket();
        }

        private async void BtnUpdateBalance_Click(object sender, RoutedEventArgs e)
        {
            var initialBalance = new Balance
            {
                BTC = 1,     
                XRP = 15000, 
                XMR = 50,   
                DASH = 30,   
                USDT = 0
            };

            
            var updatedBalance = await _mainBalance.CalculateBalanceAsync(new TickerEndpoint(), initialBalance);

            
            Balances.Clear();
            Balances.Add(updatedBalance);
        }
    }
}
