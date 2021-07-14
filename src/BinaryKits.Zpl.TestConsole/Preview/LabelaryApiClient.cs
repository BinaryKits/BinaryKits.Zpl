using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.TestConsole.Preview
{
    public class LabelaryApiClient
    {
        private readonly string _apiEndpoint;
        public LabelaryApiClient(string apiEndpoint = "http://api.labelary.com/v1/printers")
        {
            _apiEndpoint = apiEndpoint;
        }

        public async Task<byte[]> GetPreviewAsync(string zplData, PrintDensity printDensity, LabelSize labelSize)
        {
            var dpi = printDensity.ToString().Substring(2);
            var zpl = Encoding.UTF8.GetBytes(zplData);
            
            using var httpClient = new HttpClient();
            var byteContent = new ByteArrayContent(zpl);
            var response = await httpClient.PostAsync($"{_apiEndpoint}/{dpi}/labels/{labelSize.WidthInInch}x{labelSize.HeightInInch}/0/", byteContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Cannot get a preview");
                return Array.Empty<byte>();
            }

            var data = await response.Content.ReadAsByteArrayAsync();
            return data;
        }
    }
}
