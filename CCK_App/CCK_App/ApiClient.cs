using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CCK_App.Models;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Xamarin.Essentials;

namespace CCK_App
{
    public class ApiClient
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Entradas>> ApiGetTicketsAll()
        {
            HttpResponseMessage res = await client.GetAsync("http://itecno.com.ar:3000/api/cck/tickets/all");
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> ApiGetTicketsByDni(string dni)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/cck/tickets/bydni/{dni}");
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> ApiPutTicketPreshow(int idEntradas)
        {
            HttpResponseMessage res = await client.PutAsync($"http://itecno.com.ar:3000/api/cck/tickets/preshow/{idEntradas}/{CrossDeviceInfo.Current.Id}", null);
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> ApiPutTicketShow(int idEntradas)
        {
            HttpResponseMessage res = await client.PutAsync($"http://itecno.com.ar:3000/api/cck/tickets/show/{idEntradas}/{CrossDeviceInfo.Current.Id}", null);
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);

            return data;
        }

    }
}