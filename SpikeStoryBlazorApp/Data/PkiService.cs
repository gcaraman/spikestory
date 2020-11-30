using SpikeStoryBlazorApp.Data.Models;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpikeStoryBlazorApp.Data
{
    public class PkiService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public PkiService(HttpClient httpClient, JsonSerializerOptions jsonOptions)
        {
            _client = httpClient;
            _jsonOptions = jsonOptions;
        }

        public async Task<HttpResponse<byte[]>> GenerateCertificates(CertificateRequest request)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            using var httpResponse = await _client.PostAsync("SystemCertificate/Generate", stringContent).ConfigureAwait(false);

            var response = new HttpResponse<byte[]>
            {
                StatusCode = (int)httpResponse.StatusCode,
            };

            if (httpResponse.IsSuccessStatusCode)
            {
                response.Data = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
            else if ((int)httpResponse.StatusCode == 401)
            {
                response.Message = "Unauthorized";
            }
            else
            {
                response.Message = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return response;
        }

        public static string Unzip(byte[] zippedBuffer)
        {
            if (zippedBuffer == null || zippedBuffer.Length == 0)
            {
                return null;
            }
            using var zippedStream = new MemoryStream(zippedBuffer);
            using var archive = new ZipArchive(zippedStream);
            var entry = archive.Entries.FirstOrDefault();

            if (entry != null)
            {
                using var unzippedEntryStream = entry.Open();
                using var ms = new MemoryStream();
                unzippedEntryStream.CopyTo(ms);
                var unzippedArray = ms.ToArray();

                return Encoding.ASCII.GetString(unzippedArray);
            }

            return null;
        }
    }
}
