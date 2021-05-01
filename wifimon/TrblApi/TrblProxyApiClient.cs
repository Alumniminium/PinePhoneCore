using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using wifimon.TrblApi.Models;

namespace wifimon.TrblApi
{
    public class TrblProxyApiClient : TrblApiClient
    {
        public async Task<bool> UploadProxyList(string path)
        {
            var response = await httpClient.PostAsync($"{ENDPOINT}/proxy", await ToFileUpload(path));
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<BlockingCollection<Proxy>> GetProxiesAsync()
        {
            var json = await httpClient.GetStringAsync($"{ENDPOINT}/proxy");
            var proxies = JsonSerializer.Deserialize<Proxy[]>(json, serializerOptions);
            var bc = new BlockingCollection<Proxy>();
            
            foreach (var proxy in proxies)
                bc.Add(proxy);
            
            return bc;
        }
        public async Task<bool> UpdateProxyAsync(Proxy proxy)
        {
            var response = await httpClient.PatchAsync($"{ENDPOINT}/proxy", ToJsonContent(proxy));
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return response.StatusCode == HttpStatusCode.OK;
        }
        public async Task<bool> DeleteProxyAsync(Proxy proxy)
        {
            var response = await httpClient.DeleteAsync($"{ENDPOINT}/proxy?id=" + proxy.Id);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}