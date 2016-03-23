using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JukeBox.Utilities
{
    public static class GetDataToXmlFile
    {
        public static async void GetWrite(Func<string, bool> preload, string fileName, string baseAdress, string query, Action callback = null)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseAdress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    using (HttpResponseMessage response = await client.GetAsync(query))
                    {
                        response.EnsureSuccessStatusCode();
                        //try
                        //{
                            Trace.WriteLine(DateTime.Now.TimeOfDay + ": nalazim se u response zoni!");

                            if (preload(response.Content.ReadAsStringAsync().Result))
                            {
                                Trace.WriteLine(DateTime.Now.TimeOfDay + ": prosao sam validaciju datuma, spreman sam staviti novi setup!");
                                Trace.WriteLine(DateTime.Now.TimeOfDay + ": krecem zapisat setup u file!");

                                Stream dataToWrite = response.Content.ReadAsStreamAsync().Result;
                                using (FileStream DestinationStream = File.Create(fileName))
                                {
                                    //try
                                    //{
                                    await dataToWrite.CopyToAsync(DestinationStream);
                                    Trace.WriteLine(DateTime.Now.TimeOfDay + ": zapisao sam setup u file!");
                                    //}
                                    //catch (IOException)
                                    //{
                                    //    Trace.WriteLine(DateTime.Now.TimeOfDay + ": setup file se koristi, nemogu sad zapisati u njega!");
                                    //}
                                }
                                callback();
                            }
                        //}
                        //catch (Exception)
                        //{
                        //    Trace.WriteLine(DateTime.Now.TimeOfDay + ": web setup vraca null, ocito radim query prema krivom imenu");
                        //}
                    }
                }
                catch (IOException)
                {
                    Trace.WriteLine(DateTime.Now.TimeOfDay + ": setup file se koristi, nemogu sad zapisati u njega!");
                }
                catch (HttpRequestException)
                {
                    Trace.WriteLine(DateTime.Now.TimeOfDay + ": izgubio sam konekciju!");
                }
                catch (Exception)
                {
                    Trace.WriteLine(DateTime.Now.TimeOfDay + ": web setup vraca null, ocito radim query prema krivom imenu");
                }
            }
        }
    }
}
