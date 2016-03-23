using System;

namespace _6tactics.Cms.Core.Enums.Admin
{
    [Flags]
    public enum ContentType
    {
        //Header = 0,
        Project = 1,
        Language = 2,
        Page = 4,
        ContentElement = 8,
        FileElement = 16,
        Footer = 32,
        Group = 64
    }
}