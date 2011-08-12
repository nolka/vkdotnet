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
            this.Manager.Method("status.get");
            if (userId != null)
            {
                this.Manager.Params("uid", userId);
            }
            XmlNode resp = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(resp);
            return XmlUtils.String("text");
        }

        public bool Set(string status)
        {
            this.Manager.Method("status.set");
            this.Manager.Params("text", status);
            XmlNode resp = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(resp);
            return XmlUtils.BoolVal();
        }
    }
}
