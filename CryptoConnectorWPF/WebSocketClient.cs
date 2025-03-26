using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CryptoConnector.Models;
using System.Diagnostics;

namespace CryptoConnectorWPF
{
    public class WebSocketClient
    {
        private ClientWebSocket _webSocket;
        private Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Dispatcher _dispatcher;
        public event Action<Trade> OnTradeReceived;

        public WebSocketClient(Dispatcher dispatcher)
        {
            _uri = new Uri("wss://api-pub.bitfinex.com/ws/2");
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            _dispatcher = dispatcher;
        }

        public async Task ConnectAsync(string pair)
        {
            string symbol = $"t{pair}";

            if (_webSocket.State != WebSocketState.Open)
            {
                _webSocket = new ClientWebSocket();
                await _webSocket.ConnectAsync(_uri, _cancellationTokenSource.Token);
                Debug.WriteLine("WebSocket подключен.");
            }
            else
            {
                Debug.WriteLine("WebSocket уже подключен.");
            }

            if (_webSocket.State == WebSocketState.Open)
            {
                Debug.WriteLine($"Отправка запроса на подписку {symbol}");
                await SubscribeToChannel("trades", symbol);
                _ = Task.Run(ListenAsync);
            }
            else
            {
                Debug.WriteLine("Ошибка: WebSocket не подключен.");
            }
        }

        private async Task SubscribeToChannel(string channel, string key)
        {
            var subscribeMessage = new Dictionary<string, object>
            {
                { "event", "subscribe" },
                { "channel", channel },
                { "symbol", key }
            };

            var message = JsonSerializer.Serialize(subscribeMessage);
            Debug.WriteLine($"Отправка: {message}");
            await SendMessageAsync(message);
        }

        private async Task SendMessageAsync(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
        }

        private async Task ListenAsync()
        {
            var buffer = new byte[1024 * 4];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.WriteLine($"Получено сообщение: {message}");
                    ProcessMessage(message);
                }
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                if (message.Contains("tu") || message.Contains("te"))
                {
                    var tradeData = JsonSerializer.Deserialize<List<object>>(message);
                    if (tradeData != null && tradeData.Count > 3)
                    {
                        var trade = new Trade
                        {
                            Pair = "N/A",
                            Id = tradeData[1].ToString(),
                            Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(tradeData[2])),
                            Price = Convert.ToDecimal(tradeData[3]),
                            Amount = Convert.ToDecimal(tradeData[4]),
                            Side = (Convert.ToDecimal(tradeData[4]) > 0) ? "Buy" : "Sell"
                        };

                        Debug.WriteLine($"Трейд: {trade.Price} {trade.Amount} {trade.Side}");
                        _dispatcher.Invoke(() => OnTradeReceived?.Invoke(trade));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            Debug.WriteLine("Отключение WebSocket...");
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancellationTokenSource.Token);
            _webSocket.Dispose();
        }
    }
}
