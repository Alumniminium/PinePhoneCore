using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wifimon.TrblApi.Models;

namespace wifimon.TrblApi
{
    public abstract class TrblApiClient
    {
        internal const string ENDPOINT = "https://recon.her.st/api";
        internal JsonSerializerOptions serializerOptions;
        internal readonly HttpClient httpClient;

        public TrblApiClient()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
        }

        public async Task<bool> LoginAsync(string user, string pass)
        {
            var creds = new UserInfo { Username = user ?? "trbl", Password = pass ?? "herst" };
            var response = await httpClient.PostAsync($"{ENDPOINT}/auth/token", ToJsonContent(creds));
            var token = await response.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return response.StatusCode == HttpStatusCode.OK;
        }

        internal async Task<HttpContent> ToFileUpload(string path)
        {
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(path));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "file", Path.GetFileName(path));
            return form;
        }

        internal StringContent ToJsonContent(object input) => new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
    }
}