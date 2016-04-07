using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiPerms
    {
        SendNotify = 1,
        Friends = 2,
        Photos = 4,
        Audio = 8,
        Video = 16,
        Offers = 32,
        Questions = 64,
        Wiki = 128,
        SidebarLink = 256,
        WallPublisher = 512,
        UserStatus = 1024,
        UserNotes = 2048,
        ExtendedMessages = 4096,
        ExtendedWall = 8192
    }
}
