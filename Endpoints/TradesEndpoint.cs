using CryptoConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoConnector.Endpoints
{
    public class TradesEndpoint
    {

        public async Task<List<Trade>> GetTradesAsync(string pair, int maxCount)
        {
            var client = new HttpClient();
            var trades = new List<Trade>();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api-pub.bitfinex.com/v2/trades/t{pair}/hist?limit={maxCount}&sort=-1"),
                Headers =
                {
                    { "accept", "application/json" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonTrades = JsonSerializer.Deserialize<List<List<double>>>(body);
                
                foreach (var trade in jsonTrades)
                {
                    
                    trades.Add(new Trade
                    {
                        Pair = pair,
                        Price = (decimal)trade[3],
                        Amount = (decimal)trade[2],
                        Side =  ((decimal)trade[2] > 0) ? "buy" : "sell",
                        Time = DateTimeOffset.FromUnixTimeMilliseconds((long)trade[1]),
                        Id = trade[0].ToString()
                    });
                }
                return trades;

            }
        }
    }
}
