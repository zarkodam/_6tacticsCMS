using System;
using System.Web;

namespace _6tactics.Utilities.Web
{
    public class CookieHandler
    {
        private readonly string _cookieName = "";
        
        public CookieHandler(string cookieName)
        {
            _cookieName = cookieName;
        }

        public string GetCookieValue
        {
            get
            {
                return HttpContext.Current.Request.Cookies[_cookieName] != null
                ? HttpContext.Current.Request.Cookies[_cookieName].Values[_cookieName]
                : "";
            }
        }

        public void SetCookie(string cookieValue, DateTime exparationTime)
        {
            var cookie = new HttpCookie(_cookieName);
            cookie[_cookieName] = cookieValue;
            cookie.Expires = exparationTime;
            HttpContext.Current.Response.Cookies.Add(cookie); 
        }
    }
}