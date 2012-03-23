using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Status
{
    [Obsolete("Status is market as deprecated in official VK api documentation")]
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
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.String("text");
        }

        public bool Set(string status)
        {
            this.Manager.Method("status.set", new object[] { "text", status });
            this.Manager.Params("text", status);
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }
    }
}
