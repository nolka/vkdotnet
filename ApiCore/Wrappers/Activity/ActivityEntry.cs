using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Activity
{
    /// <summary>
    /// Represents an a Vkontakte activity records
    /// </summary>
    public class ActivityEntry
    {
        /// <summary>
        /// Activity id
        /// </summary>
        public int Id;
        /// <summary>
        /// Activity text
        /// </summary>
        public string Text;
        /// <summary>
        /// Activity date and time
        /// </summary>
        public DateTime Date;
    }
}
