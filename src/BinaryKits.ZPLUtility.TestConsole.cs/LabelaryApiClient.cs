using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKits.ZPLUtility.TestConsole.cs
{
    public class LabelaryApiClient
    {
        private readonly string _apiEndpoint;
        public LabelaryApiClient(string apiEndpoint = "http://api.labelary.com/v1/printers")
        {
            _apiEndpoint = apiEndpoint;
        }

        public async Task<byte[]> GetPreviewAsync(string zplText)
        {
            byte[] zpl = Encoding.UTF8.GetBytes(zplText);
            var byteContent = new ByteArrayContent(zpl);

            var widthMillimeter = 100;
            var heightMillimeter = 300;

            var widthInch = Math.Round(widthMillimeter / 25.4, 0);
            var heightInch = Math.Round(heightMillimeter / 25.4, 0);

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{_apiEndpoint}/8dpmm/labels/{widthInch}x{heightInch}/0/", byteContent);
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
