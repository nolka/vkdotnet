using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Status
{
    public class StatusFactory: BaseFactory
    {
        public StatusFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        public string Get(int? userId)
        {
            this.Manager.Method("status.get", new object[] { "uid", userId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml().FirstChild);

            return XmlUtils.String("text");
        }

        /// <summary>
        /// Устанавливает новый статус текущему пользователю. 
        /// </summary>
        /// <param name="status">текст статуса, который необходимо установить текущему пользователю. Если параметр не задан или равен пустой строке, то статус текущего пользователя будет очищен.</param>
        /// <param name="audio">текущая аудиозапись, которую необходимо транслировать в статус, задается в формате oid_aid (идентификатор владельца и идентификатор аудиозаписи, разделенные знаком подчеркивания). При указании параметра audio параметр status игнорируется.</param>
        /// <returns></returns>
        public bool Set(string status)
        {
            this.Manager.Method("status.set", new object[] { "text", status });

            this.Manager.Params("text", status);

            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }
    }
}
