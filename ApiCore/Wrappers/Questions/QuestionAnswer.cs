using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Questions
{
    /// <summary>
    /// Represents question answer
    /// </summary>
    public class QuestionAnswer
    {
        /*
         <answer>
        <aid>92587</aid>
        <qid>60505</qid>
        <uid>9154732</uid>
        <text>
        очень давно еще было предложение разделить друзей на группы)))и смайлы добавить)))
        </text>
        <num>12977</num>
        <date>2008-03-29 00:20:02</date>
        <name>Milena Siam</name>
        <photo>[http://cs210.vkontakte.ru/u9154732/c_a5805314.jpg|http://cs210.vkontakte.ru/u9154732/c_a5805314.jpg]</photo>
        <online>1</online>
        </answer>
         */
        /// <summary>
        /// Question answer id
        /// </summary>
        public int Id;
        /// <summary>
        /// Question id
        /// </summary>
        public int QuestionId;
        /// <summary>
        /// Answer user id
        /// </summary>
        public int UserId;
        /// <summary>
        /// Answer text
        /// </summary>
        public string Text;
        /// <summary>
        /// Answer number
        /// </summary>
        public int Number;
        /// <summary>
        /// Answer date
        /// </summary>
        public string Date;
        /// <summary>
        /// Answer user name
        /// </summary>
        public string UserName;
        /// <summary>
        /// Answer user photo URL
        /// </summary>
        public string UserPhoto;
        /// <summary>
        /// Us answer user online
        /// </summary>
        public bool IsUserOnline;
    }
}
