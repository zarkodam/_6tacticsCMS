using _6tactics.Utilities.Web;
using System;
using _6tactics.Cms.Core.Enums.Admin;

namespace _6tactics.Cms.Web.Helpers
{
    public static class PopupMessage
    {
        public static void SetMessage(ContentItemAction contentItemAction, MessageType messageType)
        {
            var contentItemActionCookie = new CookieHandler("ContentItemAction");
            contentItemActionCookie.SetCookie(contentItemAction.ToString(), DateTime.Now.AddMinutes(1));
            var messageTypeCookie = new CookieHandler("MessageType");
            messageTypeCookie.SetCookie(messageType.ToString(), DateTime.Now.AddMinutes(1));
        }
    }
}
