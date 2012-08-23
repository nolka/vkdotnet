using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    /// <summary>
    /// Помогает мапить ответы от api на классы библиотеки.
    /// </summary>
    public class Metadata: System.Attribute
    {
        private string node;

        public Type CastTo = null;
        public bool IsCollection = false;

        public Metadata(string node)
        {
            this.node = node;
        }

        public string GetNodeName()
        {
            return this.node;
        }
    }

    public class ClassMetadata : Metadata
    {
        public ClassMetadata(string node) : base(node) { }
    }
}
