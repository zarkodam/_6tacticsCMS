using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace _6tactics.Utilities.Network
{
    public class GetGeoIp
    {
        private readonly string _externalIp = "";

        public GetGeoIp()
        {
            //_externalIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (_externalIp == "")
            //    _externalIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            _externalIp = HttpContext.Current.Request.UserHostAddress;
        }

        public string GetJsonStringResult()
        {
            WebRequest request = WebRequest.Create(string.Concat("http://freegeoip.net/json/", _externalIp != "" ? _externalIp : "", "/"));
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (receiveStream != null)
                        readStream = new StreamReader(receiveStream);
                    if (readStream != null)
                        return readStream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return "";
        }

        public XDocument GetXmlResult()
        {
            WebRequest request = WebRequest.Create(string.Concat("http://freegeoip.net/xml/", _externalIp != "" ? _externalIp : "", "/"));
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    var receiveXml = new XmlTextReader(response.GetResponseStream());
                    return XDocument.Load(receiveXml);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return null;
        }

        public Dictionary<string, string> GetDictionaryResult()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(GetJsonStringResult());
        }
    }

}

//USAGE:
//GetGeoIp getGeoIp = new GetGeoIp();
//Trace.WriteLine(getGeoIp.GetJsonStringResult());
//Trace.WriteLine(getGeoIp.GetXmlResult().ToString());
//Trace.WriteLine(getGeoIp.GetDictionaryResult().SingleOrDefault(k => k.Key == "country_name").Value);
//foreach (KeyValuePair<string, string> item in getGeoIp.GetDictionaryResult())
//Trace.WriteLine(item.Key + ": " + item.Value);

//{"ip":"93.138.70.190","country_code":"HR","country_name":"Croatia","region_code":"21","region_name":"Grad Zagreb","city":"Zagreb","zipcode":"","latitude":45.8,"longitude":16,"metro_code":"","areacode":""}

//<Response>
// <Ip>93.138.70.190</Ip>
// <CountryCode>HR</CountryCode>
// <CountryName>Croatia</CountryName>
// <RegionCode>21</RegionCode>
// <RegionName>Grad Zagreb</RegionName>
// <City>Zagreb</City>
// <ZipCode></ZipCode>
// <Latitude>45.8</Latitude>
// <Longitude>16</Longitude>
// <MetroCode></MetroCode>
// <AreaCode></AreaCode>
//</Response>