using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CCK_App.Models;
using Newtonsoft.Json;
using CCK_App.Models;

namespace CCK_QR_App
{
    public class ApiClient
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Entradas>> ApiGetTicketsAll()
        {
            HttpResponseMessage res = await client.GetAsync("http://itecno.com.ar:3000/api/cck/tickets/all");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> ApiGetTicketsByDni(string dni)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/cck/tickets/bydni/{dni}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> ApiPutTicketIngresado(string dni, int idEventos, DateTime dt)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/cck/tickets/ingresado/{dni}/{idEventos}/{dt}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }

    }
}