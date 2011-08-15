using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    public static class XmlUtils
    {
        private static XmlNode node;

        /// <summary>
        /// Set active node
        /// </summary>
        /// <param name="node">node to be used as active</param>
        public static void UseNode(XmlNode node)
        {
            XmlUtils.node = node;
        }
        
        /// <summary>
        /// Gets the string value from node
        /// </summary>
        /// <param name="nodeName">node</param>
        /// <returns>string</returns>
        public static string String(string nodeName)
        {
            if (XmlUtils.node.SelectSingleNode(nodeName) != null)
            {
                return XmlUtils.node.SelectSingleNode(nodeName).InnerText
                                                                .Replace("&lt;br&gt;", "\r\n");

            }
            return "";
        }

        /// <summary>
        /// Gets the int value from node
        /// </summary>
        /// <param name="nodeName">node</param>
        /// <returns>int</returns>
        public static int Int(string nodeName)
        {
            if (XmlUtils.node.SelectSingleNode(nodeName) != null)
            {
                return Convert.ToInt32(XmlUtils.node.SelectSingleNode(nodeName).InnerText);
            }
            return -1;
        }

        public static int IntVal()
        {
            if (XmlUtils.node != null)
            {
                return Convert.ToInt32(XmlUtils.node.InnerText) ;
            }
            return -1;
        }

        /// <summary>
        /// Gets double value from node
        /// </summary>
        /// <param name="nodeName">node</param>
        /// <returns>double</returns>
        public static double Double(string nodeName)
        {
            if (XmlUtils.node.SelectSingleNode(nodeName) != null)
            {
                return Convert.ToDouble(XmlUtils.node.SelectSingleNode(nodeName).InnerText);
            }
            return -1;
        }

        /// <summary>
        /// Gets float value from node
        /// </summary>
        /// <param name="nodeName">node</param>
        /// <returns>float</returns>
        public static float Float(string nodeName)
        {
            if (XmlUtils.node.SelectSingleNode(nodeName) != null)
            {
                return float.Parse(XmlUtils.node.SelectSingleNode(nodeName).InnerText);
            }
            return -1;
        }

        /// <summary>
        /// Gets the bool value from node
        /// </summary>
        /// <param name="nodeName">node</param>
        /// <returns>true or false</returns>
        public static bool Bool(string nodeName)
        {
            if (XmlUtils.node.SelectSingleNode(nodeName) != null)
            {
                return ((XmlUtils.node.SelectSingleNode(nodeName).InnerText == "1") ? true : false);
            }
            return false;
        }

        public static bool BoolVal()
        {
            if (XmlUtils.node != null)
            {
                return ((XmlUtils.node.InnerText == "1") ? true : false);
            }
            return false;
        }
    }
}
