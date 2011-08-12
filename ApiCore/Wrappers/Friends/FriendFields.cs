using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Friends
{
    public static class FriendFields
    {
        /*
          uid, first_name, last_name, nickname, sex, bdate (birthdate), city, 
         * country, timezone, photo, photo_medium, photo_big, online, lists, domain. 
         * Если в параметре fields было указано поле contacts, то будут доступны 
         * также поля: home_phone, mobile_phone. 
         * Если в параметре fields было указано поле education, 
         * то будут доступны также поля: university, university_name, faculty, faculty_name, graduation
         */
        /// <summary>
        /// User id
        /// </summary>
        public static readonly string Id = "uid";
        /// <summary>
        /// User first name
        /// </summary>
        public static readonly string FirstName = "first_name";
        /// <summary>
        /// User last name
        /// </summary>
        public static readonly string LastName = "last_name";
        /// <summary>
        /// User nickname
        /// </summary>
        public static readonly string NickName = "nickname";
        /// <summary>
        /// User rating
        /// </summary>
        public static readonly string Rating = "rating";
        /// <summary>
        /// User sex
        /// </summary>
        public static readonly string Sex = "sex";
        /// <summary>
        /// User birthdate
        /// </summary>
        public static readonly string BirthDate = "bdate";
        /// <summary>
        /// User city id
        /// </summary>
        public static readonly string City = "city";
        /// <summary>
        /// User country id
        /// </summary>
        public static readonly string Country = "country";
        /// <summary>
        /// User timezone
        /// </summary>
        public static readonly string Timezone = "timezone";
        /// <summary>
        /// User photo
        /// </summary>
        public static readonly string Photo = "photo";
        /// <summary>
        /// User medium photo
        /// </summary>
        public static readonly string PhotoMedium = "photo_medium";
        /// <summary>
        /// User big photo
        /// </summary>
        public static readonly string PhotoBig = "photo_big";
        /// <summary>
        /// Is user online
        /// </summary>
        public static readonly string Online = "online";
        /// <summary>
        /// User added to this lists
        /// </summary>
        public static readonly string Lists = "lists";
        /// <summary>
        /// User domain. e.g. nolka.vkontakte.ru
        /// </summary>
        public static readonly string Domain = "domain";
        /// <summary>
        /// User home phone
        /// </summary>
        public static readonly string HomePhone = "home_phone";
        /// <summary>
        /// User mobile phone
        /// </summary>
        public static readonly string MobilePhone = "mobile_phone";
        /// <summary>
        /// Is user mobile phone available
        /// </summary>
        public static readonly string HasMobile = "has_mobile";
        /// <summary>
        /// User university id
        /// </summary>
        public static readonly string University = "university";
        /// <summary>
        /// User university name
        /// </summary>
        public static readonly string UniversityName = "university_name";
        /// <summary>
        /// User faculty id
        /// </summary>
        public static readonly string Faculty = "faculty";
        /// <summary>
        /// User faculty name
        /// </summary>
        public static readonly string FacultyName = "faculty_name";
        /// <summary>
        /// User graduation
        /// </summary>
        public static readonly string Graduation = "graduation";
    }
}
