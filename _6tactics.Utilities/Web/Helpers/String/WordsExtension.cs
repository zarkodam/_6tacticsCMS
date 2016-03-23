using System.Web;
using System.Web.Mvc;

namespace _6tactics.Utilities.Web.Helpers.String
{
    public static class WordsExtension
    {
        public static IHtmlString WordsConnecting(this HtmlHelper helper, string text)
        {
            var connectedWords = text.Replace(" ", "-");
            return helper.Raw(connectedWords);
        }

        public static IHtmlString AddSpaceByLength(this HtmlHelper helper, int spaceLength)
        {
            string htmlTextWithSpace = "";

            for (int i = 0; i < spaceLength; i++)
                htmlTextWithSpace += "&nbsp;";

            return helper.Raw(htmlTextWithSpace);
        }

        public static IHtmlString AddSpaceByLengthBeforeText(this HtmlHelper helper, int spaceLength, string text)
        {
            string htmlTextWithSpace = "";
            
            for (int i = 0; i < spaceLength; i++)
                htmlTextWithSpace += "&nbsp;";

            htmlTextWithSpace += text;
            return helper.Raw(htmlTextWithSpace);
        }

        public static IHtmlString AddSpaceByLengthAfterText(this HtmlHelper helper, int spaceLength, string text)
        {
            string htmlTextWithSpace = text;

            for (int i = 0; i < spaceLength; i++)
                htmlTextWithSpace += "&nbsp;";

            return helper.Raw(htmlTextWithSpace);
        }
    }
}
