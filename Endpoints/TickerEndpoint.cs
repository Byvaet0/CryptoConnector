using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoConnector.BitfinexClient
{
    public class TickerEndpoint
    {

        public async Task<string> GetTickerAsync(string pair)
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
                return body;
            }
        }

    }
}
