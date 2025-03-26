using Microsoft.AspNetCore.Mvc;
using CryptoConnector.Endpoints;
using CryptoConnector.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CandleController : ControllerBase
{
    private readonly CandleEndpoint _candleEndpoint = new();

    [HttpGet("{pair}")]
    public async Task<ActionResult<List<Candle>>> GetCandle(string pair)
    {
        var candles = await _candleEndpoint.GetCandleAsync(pair);
        return Ok(candles);
    }
}

[Route("api/[controller]")]
[ApiController]
public class TickerController : ControllerBase
{
    private readonly TickerEndpoint _tickerEndpoint = new();

    [HttpGet("{pair}")]
    public async Task<ActionResult<Ticker>> GetTicker(string pair)
    {
        var ticker = await _tickerEndpoint.GetTickerAsync(pair);
        return Ok(ticker);
    }
}

[Route("api/[controller]")]
[ApiController]
public class TradesController : ControllerBase
{
    private readonly TradesEndpoint _tradesEndpoint = new();

    [HttpGet("{pair}/{maxCount}")]
    public async Task<ActionResult<List<Trade>>> GetTrades(string pair, int maxCount)
    {
        var trades = await _tradesEndpoint.GetTradesAsync(pair, maxCount);
        return Ok(trades);
    }
}
