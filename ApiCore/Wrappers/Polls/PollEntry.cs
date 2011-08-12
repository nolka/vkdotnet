using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Polls
{
    public class PollEntry
    {
        /// <summary>
        /// Poll id
        /// </summary>
        public int Id;
        /// <summary>
        /// Poll owner id
        /// </summary>
        public int OwnerId;
        /// <summary>
        /// Date, when poll was created
        /// </summary>
        public DateTime DateCreated;
        /// <summary>
        /// Poll question
        /// </summary>
        public string Question;
        /// <summary>
        /// Poll total answers count
        /// </summary>
        public int Votes;
        /// <summary>
        /// Poll answer id, selected by current user
        /// </summary>
        public int AnswerId;
        /// <summary>
        /// Current poll answers collection
        /// </summary>
        public PollAnswerCollection Answers;
    }
}
