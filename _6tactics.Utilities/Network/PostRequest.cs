using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace _6tactics.Utilities.Network
{
    public static class PostRequest
    {
        public static async void PostSend(string baseAdress, string postResponeAdress, object classToSend)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseAdress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    using (HttpResponseMessage response = await client.PostAsJsonAsync(postResponeAdress, classToSend))
                        if (!response.IsSuccessStatusCode)
                            Trace.WriteLine(DateTime.Now.TimeOfDay + ":Request wasn't passed");
                }
                catch (HttpRequestException e)
                {
                    Trace.WriteLine(e.Message);
                    Trace.WriteLine(DateTime.Now.TimeOfDay + ":NO CONNECTION FOR POST REQUEST");
                }
                catch (TaskCanceledException e)
                {
                    Trace.WriteLine(DateTime.Now.TimeOfDay + ": " + e.Message);
                }

            }
        }
    }
}
