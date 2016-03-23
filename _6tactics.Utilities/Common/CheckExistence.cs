using _6tactics.Utilities.StringUtilities;
using _6tactics.Utilities.Web;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace _6tactics.Utilities.Common
{
    public class CheckExistence
    {
        private static bool IsPathPublic(string url)
        {
            return url.ToLower().CustomContains("http") || url.ToLower().CustomContains("www.");
        }

        private static string CreateUrl(string url)
        {
            return IsPathPublic(url) ? url : url.CreateLocalUrl();
        }

        private static bool CheckUrl(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                try
                {
                    request.Method = WebRequestMethods.Http.Head;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    bool isPageExists = response.StatusCode == HttpStatusCode.OK;
                    response.Close();
                    return isPageExists;

                }
                catch (Exception ex)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine("URL: " + url + " checked through GET request!\n" + ex.Message);
                            return response.StatusCode == HttpStatusCode.OK;
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("URL: " + url + " don't exist! \n" + ex.Message);
                return false;
            }
        }

        public static bool IsUrlExist(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            return CheckUrl(CreateUrl(url).AddProtocolToUrl("http")) ||
                   CheckUrl(CreateUrl(url).AddProtocolToUrl("https"));
        }
    }
}
