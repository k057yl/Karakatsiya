using Newtonsoft.Json.Linq;

public class CurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
    {
        var result = new Dictionary<string, decimal>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JArray jsonRates = JArray.Parse(jsonResponse);

            var usdRate = jsonRates.FirstOrDefault(rate => rate["cc"]?.ToString() == "USD");
            if (usdRate != null)
            {
                result["USD"] = Math.Round(usdRate["rate"]?.ToObject<decimal>() ?? 0, 2);
            }

            var eurRate = jsonRates.FirstOrDefault(rate => rate["cc"]?.ToString() == "EUR");
            if (eurRate != null)
            {
                result["EUR"] = Math.Round(eurRate["rate"]?.ToObject<decimal>() ?? 0, 2);
            }
        }
        catch (Exception)
        {
            result["USD"] = 0;
            result["EUR"] = 0;
        }

        return result;
    }
}