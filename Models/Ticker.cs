using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoConnector.Models
{
    public class Ticker
    {
        public string Pair { get; set; }

        // Price of last highest bid
        public decimal Bid { get; set; }  
        
        // Sum of the 25 highest bid sizes
        public decimal BidSize { get; set; }

        // Price of last lowest ask
        public decimal Ask { get; set; }

        // 	Sum of the 25 lowest ask sizes
        public decimal AskSize { get; set; }

        // Price of the last trade
        public decimal LastPrice { get; set; }

        // Amount that the last price has changed since yesterday
        public decimal DailyChange { get; set; }

        // Relative price change since yesterday (*100 for percentage change)
        public decimal DailyChangePercent { get; set; }

        // Daily high
        public decimal High { get; set; }

        // Daily low
        public decimal Low { get; set; }

        // Daily volume
        public decimal Volume { get; set; }        
    }
}
