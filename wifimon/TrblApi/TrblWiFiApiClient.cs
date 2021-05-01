using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using wifimon.TrblApi.Models;

namespace wifimon.TrblApi
{
    public class TrblWiFiApiClient : TrblApiClient
    {
        public async Task<bool> SubmitProbe(WiFiProbe probe)
        {
            var response = await httpClient.PutAsync($"{ENDPOINT}/wifi/probe",ToJsonContent(probe));
            if(response.StatusCode != HttpStatusCode.OK)
                return false;
            var dto = JsonSerializer.Deserialize<PutResponseDto>(await response.Content.ReadAsStringAsync());
            Console.WriteLine(dto);
            return true;
        }
        public async Task<bool> SubmitBeacon(WiFiAccessPoint ap)
        {
            var response = await httpClient.PutAsync($"{ENDPOINT}/wifi/accesspoint",ToJsonContent(ap));
            if(response.StatusCode != HttpStatusCode.OK)
                return false;
            var dto = JsonSerializer.Deserialize<PutResponseDto>(await response.Content.ReadAsStringAsync());
            Console.WriteLine(dto);
            return true;
        }
    }
}