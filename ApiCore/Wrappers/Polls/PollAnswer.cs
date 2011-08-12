using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Polls
{
    /// <summary>
    /// 
    /// </summary>
    public class PollAnswer
    {
        /// <summary>
        /// Answer id
        /// </summary>
        public int Id;
        /// <summary>
        /// Number of answers
        /// </summary>
        public int Votes;
        /// <summary>
        /// Answer text
        /// </summary>
        public string Text;
        /// <summary>
        /// Answer rating
        /// </summary>
        public float Rate;
    }
}
