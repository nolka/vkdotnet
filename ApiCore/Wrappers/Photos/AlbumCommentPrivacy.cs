using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Photos
{
    public enum AlbumCommentPrivacy
    {
        All = 0,
        OnlyFriends = 1,
        FriendsOfFriends = 2,
        OnlyMe = 3
    }
}
