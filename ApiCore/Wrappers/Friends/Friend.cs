using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Friends
{
    public class Friend
    {
        /// <summary>
        /// User id
        /// </summary>
        public int Id;
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName;
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName;
        /// <summary>
        /// User nickname
        /// </summary>
        public string NickName;
        /// <summary>
        /// User rating
        /// </summary>
        public int Rating;
        /// <summary>
        /// User sex
        /// </summary>
        public FriendSex Sex;
        /// <summary>
        /// User birthdate
        /// </summary>
        public string BirthDate;
        /// <summary>
        /// User city id
        /// </summary>
        public int City;
        /// <summary>
        /// User country id
        /// </summary>
        public int Country;
        /// <summary>
        /// User timezone
        /// </summary>
        public string Timezone;
        /// <summary>
        /// User photo
        /// </summary>
        public string Photo;
        /// <summary>
        /// User medium photo
        /// </summary>
        public string PhotoMedium;
        /// <summary>
        /// User big photo
        /// </summary>
        public string PhotoBig;
        /// <summary>
        /// Is user online
        /// </summary>
        public bool Online;
        /// <summary>
        /// User added to this lists
        /// </summary>
        public List<int> Lists;
        /// <summary>
        /// User domain. e.g. nolka.vkontakte.ru
        /// </summary>
        public string Domain;
        /// <summary>
        /// User home phone
        /// </summary>
        public string HomePhone;
        /// <summary>
        /// User mobile phone
        /// </summary>
        public string MobilePhone;
        /// <summary>
        /// Is user mobile phone available
        /// </summary>
        public bool HasMobile;
        /// <summary>
        /// User university id
        /// </summary>
        public int University;
        /// <summary>
        /// User university name
        /// </summary>
        public string UniversityName;
        /// <summary>
        /// User faculty id
        /// </summary>
        public int Faculty;
        /// <summary>
        /// User faculty name
        /// </summary>
        public string FacultyName;
        /// <summary>
        /// User graduation
        /// </summary>
        public int Graduation;

        public override string ToString()
        {
 	         return this.FirstName+" "+this.LastName;
        }
    }
}
