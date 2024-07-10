using System;
using System.Net.Http;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<FMPService> _logger;

        public FMPService(HttpClient httpClient, IConfiguration config, ILogger<FMPService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Stock symbol cannot be null or empty", nameof(symbol));
            }

            try
            {
                var apiKey = _config["FMPKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("API key for Financial Modeling Prep is not configured.");
                    return null;
                }

                var url = $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={apiKey}";
                var result = await _httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var convertContent = JsonConvert.DeserializeObject<FMPStockDTO[]>(content);
                    var stock = convertContent?.FirstOrDefault();
                    if (stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }
                    _logger.LogWarning("Stock data is null for symbol: {Symbol}", symbol);
                    return null;
                }
                else
                {
                    _logger.LogError("Failed to retrieve stock data. Status Code: {StatusCode}", result.StatusCode);
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "HTTP request error while retrieving stock data for symbol: {Symbol}", symbol);
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving stock data for symbol: {Symbol}", symbol);
                return null;
            }
        }
    }
}
