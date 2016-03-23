using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace _6tactics.Utilities.Network
{
    public class PutRequest
    {
        public static async void PutSend(string baseAdress, string putResponeAdress, object classToSend)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                try
                {
                    using (HttpResponseMessage response = await client.PutAsJsonAsync(putResponeAdress, classToSend))
                        if (!response.IsSuccessStatusCode)
                            Trace.WriteLine("PUT Request wasn't passed");
                }
                catch (HttpRequestException e)
                {
                    Trace.WriteLine(e.Message);
                    Trace.WriteLine("NO CONNECTION FOR POST REQUEST");
                }
            }
        }
    }
}
