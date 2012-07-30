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
        UserStatus = 1204,
        UserNotes = 2048,
        ExtendedMessages = 4096,
        ExtendedWall = 8192,
        Ads = 32768,
        Documents = 131072,
        Groups = 262144,
        Notifications = 524288
    }

    public static class ScopeSettings
    {
        /*
         * notify	Пользователь разрешил отправлять ему уведомления.
            friends	Доступ к друзьям.
            photos	Доступ к фотографиям.
            audio	Доступ к аудиозаписям.
            video	Доступ к видеозаписям.
            docs	Доступ к документам.
            notes	Доступ заметкам пользователя.
            pages	Доступ к wiki-страницам.
            offers	Доступ к предложениям (устаревшие методы).
            questions	Доступ к вопросам (устаревшие методы).
            wall	Доступ к обычным и расширенным методам работы со стеной.
            messages	(для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.
            ads	Доступ к расширенным методам работы с рекламным API.
            offline	Доступ к API в любое время со стороннего сервера.
         * */
        public static readonly string Notify = "notify";
        public static readonly string Friends = "friends";
        public static readonly string Photos = "photos";
        public static readonly string Audio = "audio";
        public static readonly string Video = "video";
        public static readonly string Documents = "docs";
        public static readonly string Notes = "notes";
        public static readonly string Pages = "pages";
        public static readonly string Offers = "offers";
        public static readonly string Questions = "questions";
        public static readonly string Wall = "wall";
        public static readonly string Messages = "messages";
        public static readonly string Ads = "ads";
        public static readonly string Offline = "offline";
        public static readonly string NoHttps = "nohttps";
    }
}
