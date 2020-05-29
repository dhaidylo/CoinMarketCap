using System.Drawing;

namespace CoinMarketCap
{
    public class CurrencyInfo
    {
        public readonly Image Logo;
        public readonly string Name;
        public readonly string Website;
        public readonly string Description;
        public readonly string Price;
        public readonly string PriceChange24h;

        public CurrencyInfo(Image logo, string name, string site, string description, string price, string priceChange24h)
        {
            Logo = logo;
            Name = name;
            Website = site;
            Description = description;
            Price = price;
            PriceChange24h = priceChange24h;
        }
    }
}
