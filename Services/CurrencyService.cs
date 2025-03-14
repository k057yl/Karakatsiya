﻿using Newtonsoft.Json.Linq;

namespace Karakatsiya.Services
{
    public class CurrencyService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:CurrencyApiKey"];
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var result = new Dictionary<string, decimal>();
            string urlUsd = $"https://v6.exchangerate-api.com/v6/{_apiKey}/pair/USD/UAH";
            string urlEur = $"https://v6.exchangerate-api.com/v6/{_apiKey}/pair/EUR/UAH";

            HttpResponseMessage responseUsd = await _httpClient.GetAsync(urlUsd);
            responseUsd.EnsureSuccessStatusCode();
            string jsonResponseUsd = await responseUsd.Content.ReadAsStringAsync();
            JObject jsonUsd = JObject.Parse(jsonResponseUsd);
            decimal usdRate = (decimal)jsonUsd["conversion_rate"];
            result["USD"] = Math.Round(usdRate, 2);

            HttpResponseMessage responseEur = await _httpClient.GetAsync(urlEur);
            responseEur.EnsureSuccessStatusCode();
            string jsonResponseEur = await responseEur.Content.ReadAsStringAsync();
            JObject jsonEur = JObject.Parse(jsonResponseEur);
            decimal eurRate = (decimal)jsonEur["conversion_rate"];
            result["EUR"] = Math.Round(eurRate, 2);

            return result;
        }
    }
}
