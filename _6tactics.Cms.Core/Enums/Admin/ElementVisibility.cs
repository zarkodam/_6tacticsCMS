using System;

namespace _6tactics.Cms.Core.Enums.Admin
{
    [Flags]
    public enum ElementVisibility
    {
        Visible = 0,
        VisibleForAdminOnly = 1,
        Hidden = 2
    }
}
