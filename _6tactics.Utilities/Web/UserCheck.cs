
namespace _6tactics.Utilities.Web
{
    public static class UserCheck
    {
        public static bool IsRoot(string userName)
        {
            return userName.Equals("root");
        }

        public static bool IsAdmin(string userName)
        {
            return userName.Equals("root") || userName.Equals("admin") || userName.Equals("administrator");
        }

        public static bool IsViewForRootAdmin(string currentSelected, string userName)
        {
            return ((currentSelected.Equals("admin") || currentSelected.Equals("administrator")) && !IsAdmin(userName))
                   || (currentSelected.Equals("root") && !IsRoot(userName));
        }
    }
}