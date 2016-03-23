namespace _6tactics.Utilities.Web.Helpers.Pjax
{
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlHelperExtensions
    {
        public static IHtmlString PageTitle(this HtmlHelper instance, string titleAddOn = "")
        {
            var title = instance.ViewBag.Title == null ? string.Empty : string.Concat(instance.ViewBag.Title, " ", titleAddOn);
            
            return instance.Raw("<title>" + title + "</title>");
        }
    }
}