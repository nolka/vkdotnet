using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public enum AlbumCommentPrivacy
    {
        All = 0,
        OnlyFriends = 1,
        FriendsOfFriends = 2,
        OnlyMe = 3
    }
}
