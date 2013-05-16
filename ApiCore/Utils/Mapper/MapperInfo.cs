using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Utils.Mapper
{
    /// <summary>
    /// Помогает мапить ответы от api на классы библиотеки.
    /// </summary>
    public class MapperInfo: System.Attribute
    {
        private string node;

        public Type CastTo = typeof(string);
        public bool IsCollection = false;

        public MapperInfo(string node)
        {
            this.node = node;
        }

        public string GetNodeName()
        {
            return this.node;
        }
    }

    public class ClassMetadata : MapperInfo
    {
        public ClassMetadata(string node) : base(node) { }
    }
}
