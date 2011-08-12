using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;


namespace ApiCore
{
    public class ApiRequestErrorException : Exception
    {
        public int Code;
        public string Description;
        public Hashtable ParamsPassed;

        public ApiRequestErrorException(string description, int code, Hashtable paramsPassed)
            : base(description)
        {
            this.Code = code;
            this.Description = description;
            this.ParamsPassed = paramsPassed;
        }

        public ApiRequestErrorException()
        {
        }

        public ApiRequestErrorException(string message)
            : base(message)
        {
        }

        public ApiRequestErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string ToString()
        {
            return String.Format("code: {0}, message: {1}", this.Code, this.Description);
        }

    }
}
