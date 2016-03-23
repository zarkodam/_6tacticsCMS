using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Popup;
using System.Web;

namespace _6tactics.Cms.Core.Utilities
{
    public static class PopupMessageUtility
    {
        public static Popup GetMessage()
        {
            return (Popup)HttpContext.Current.Session["Popup"];
        }

        public static void SetMessage(ContentItemAction contentItemAction, MessageType messageType)
        {
            HttpContext.Current.Session["Popup"] = PopupMessageModelGenerator.Generate(contentItemAction, messageType);
        }

        public static void RemoveMessage()
        {
            HttpContext.Current.Session["Popup"] = null;
        }
    }
}
