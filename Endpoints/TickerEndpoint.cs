using CryptoConnector.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoConnector.BitfinexClient
{
    public class TickerEndpoint
    {

        public async Task<Ticker> GetTickerAsync(string pair)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api-pub.bitfinex.com/v2/ticker/t{pair}"),
                Headers =
                {
                    { "accept", "application/json" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonTicker = JsonSerializer.Deserialize<List<double>>(body);

                return new Ticker
                {
                    Pair = pair,
                    Bid = (decimal)jsonTicker[0],
                    BidSize = (decimal)jsonTicker[1],
                    Ask = (decimal)jsonTicker[2],
                    AskSize = (decimal)jsonTicker[3],
                    LastPrice = (decimal)jsonTicker[6],
                    DailyChange = (decimal)jsonTicker[4],
                    DailyChangePercent = (decimal)jsonTicker[5],
                    High = (decimal)jsonTicker[8],
                    Low = (decimal)jsonTicker[9],
                    Volume = (decimal)jsonTicker[7]
                };
            }
        }

    }
}
