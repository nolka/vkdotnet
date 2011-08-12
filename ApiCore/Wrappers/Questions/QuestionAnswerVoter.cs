using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Questions
{
    public class QuestionAnswerVoter
    {
        /*
         <vote>
        <vid>8017625</vid>
        <voter_id>6704204</voter_id>
        <date>2008-10-30 03:49:35</date>
        </vote>
         */
        /// <summary>
        /// Vote id
        /// </summary>
        public int Id;
        /// <summary>
        /// Voter id
        /// </summary>
        public int VoterId;
        /// <summary>
        /// Vote date
        /// </summary>
        public string Date;
        /// <summary>
        /// Voter name
        /// </summary>
        public string Name;
        /// <summary>
        /// Voter photo URL
        /// </summary>
        public string Photo;
        /// <summary>
        /// Is voter online
        /// </summary>
        public bool IsUserOnline;
    }
}
