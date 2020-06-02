using System;
using System.IO;
using System.Drawing;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace CoinMarketCap
{
    public class CoinMarketCap : IDisposable
    {
        private const string TEMP_PATH = "temp";
        private const string API_KEY = "api_key";

        public CoinMarketCap()
        {
            Directory.CreateDirectory(TEMP_PATH);
        }

        public string GetPage(int page, int onPage = 200)
        {
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var start = (page - 1) * onPage;
            if (start > 0)
                queryString["start"] = start.ToString();
            queryString["limit"] = onPage.ToString();

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(URL.ToString());
        }

        private string GetMetadataById(string id)
        {
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/info");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = id;

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(URL.ToString());
        }

        private string GetDataByID(string id)
        {
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = id;

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(URL.ToString());
        }

        public CurrencyInfo GetCurrencyInfoById(string id)
        {
            dynamic meta = JsonConvert.DeserializeObject(GetMetadataById(id));
            var metadata = meta.data;

            dynamic obj = JsonConvert.DeserializeObject(GetDataByID(id));
            var data = obj.data;

            var logoPath = $@"{TEMP_PATH}\{id}.png";
            if (!File.Exists(logoPath))
                using (var client = new WebClient())
                    client.DownloadFile(new Uri(metadata[id].logo.ToString()), logoPath);

            return new CurrencyInfo(
                Image.FromFile(logoPath),
                metadata[id].name.ToString(),
                metadata[id].urls.website[0].ToString(),
                metadata[id].description.ToString(),
                data[id].quote.USD.price.ToString(),
                data[id].quote.USD.percent_change_24h.ToString()
                );
        }

        public void Dispose()
        {
            Directory.Delete(TEMP_PATH, true);
        }
    }
}
