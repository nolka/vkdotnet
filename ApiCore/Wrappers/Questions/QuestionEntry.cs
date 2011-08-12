using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Questions
{
    public class QuestionEntry
    {
        /*
        <question>
        <qid>60505</qid>
        <uid>1</uid>
        <type>1</type>
        <text>Что бы Вы хотели добавить на сайт?</text>
        <answers_num>184037</answers_num>
        <last_poster_id>51881707</last_poster_id>
        <last_poster_name>Альбина Соснова</last_poster_name>
        <last_post_date>2009-10-16 05:22:46</last_post_date>
        <date>2008-03-29 00:18:47</date>
        <name>Павел Дуров</name>
        <photo>httр://cs136.vkоntakte.ru/u00001/c_1171579f.jpg</photo>
        <online>0</online>
        </question> 
         */
        /// <summary>
        /// Question id
        /// </summary>
        public int Id;
        /// <summary>
        /// User id
        /// </summary>
        public int UserId;
        /// <summary>
        /// Question type
        /// </summary>
        public int Type;
        /// <summary>
        /// Question type info
        /// </summary>
        public QuestionType TypeInfo;
        /// <summary>
        /// Question text
        /// </summary>
        public string Text;
        /// <summary>
        /// Answers to question count
        /// </summary>
        public int AnswersCount;
        /// <summary>
        /// Last poster id 
        /// </summary>
        public int LastPosterId;
        /// <summary>
        /// Last poster name
        /// </summary>
        public string LastPosterName;
        /// <summary>
        /// Last poster date
        /// </summary>
        public string LastPosterDate;
        /// <summary>
        /// Question post date
        /// </summary>
        public string Date;
        /// <summary>
        /// Question user name
        /// </summary>
        public string UserName;
        /// <summary>
        /// Question user photo URL
        /// </summary>
        public string UserPhoto;
        /// <summary>
        /// Is question author online
        /// </summary>
        public bool IsUserOnline;
    }
}
