using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public class ApiRequestEmptyAnswerException: Exception
    {

        public ApiRequestEmptyAnswerException()
        {
        }

        public ApiRequestEmptyAnswerException(string message)
        : base(message)
        {
        }

        public ApiRequestEmptyAnswerException(string message, Exception inner)
        : base(message, inner)
        {
        }

        public override string ToString()
        {
            return String.Format("Server returns an empty answer");
        }
    }
}
