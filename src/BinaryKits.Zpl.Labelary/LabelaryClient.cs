using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.Labelary
{
    public class LabelaryClient : IDisposable
    {
        private readonly ILogger<LabelaryClient> _logger;
        private readonly string _apiEndpoint;
        private readonly HttpClient _httpClient;

        public LabelaryClient(
            ILogger<LabelaryClient> logger = default,
            string apiEndpoint = "http://api.labelary.com/v1/printers")
        {
            _logger = logger;
            _apiEndpoint = apiEndpoint;
            _httpClient = new HttpClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);  // Violates rule
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
        }

        public async Task<byte[]> GetPreviewAsync(
            string zplData,
            PrintDensity printDensity,
            LabelSize labelSize)
        {
            var dpi = printDensity.ToString().Substring(2);
            var zpl = Encoding.UTF8.GetBytes(zplData);

            var byteContent = new ByteArrayContent(zpl);
            using (var response = await _httpClient.PostAsync($"{_apiEndpoint}/{dpi}/labels/{labelSize.WidthInInch}x{labelSize.HeightInInch}/0/", byteContent))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogError($"Cannot get a preview {response.StatusCode}");
                    return Array.Empty<byte>();
                }

                var data = await response.Content.ReadAsByteArrayAsync();
                return data;
            }
        }
    }
}
