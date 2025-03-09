using CryptoConnector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoConnector.Endpoints
{
    public class CandleEndpoint
    {


        public async Task<List<Candle>> GetCandleAsync(string pair)
        {
            var candles = new List<Candle>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api-pub.bitfinex.com/v2/candles/trade%3A1m%3At{pair}/hist"),
                Headers =
                {
                     { "accept", "application/json" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonCandles = JsonSerializer.Deserialize<List<List<double>>>(body);

                foreach (var candle in jsonCandles)
                {

                    candles.Add(new Candle
                    {
                        Pair = pair,
                        OpenPrice = (decimal)candle[1],
                        HighPrice = (decimal)candle[3],
                        LowPrice = (decimal)candle[4],
                        ClosePrice = (decimal)candle[2],
                        TotalPrice = (((decimal)candle[3] + (decimal)candle[4]) / 2) * (decimal)candle[5], //TotalPrice = Средняя цена * TotalVolume
                        TotalVolume = (decimal)candle[5],
                        OpenTime = DateTimeOffset.FromUnixTimeMilliseconds((long)candle[0])
                    });
                }
                return candles;
            }
        }
    }
}
