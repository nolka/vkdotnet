using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Utils.Mapper
{
    interface IResponseMapper
    {
       object Map(string response, Type mapToClass);
    }
}
