using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using ApiCore.Utils.Mapper;

namespace ApiCore.Utils
{
    public class EntityBuilder
    {
        public DateTime FromUnixTime(int time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time));
        }

        public string[] IntArrayToString(int[] integers)
        {
            string[] arr = new string[integers.Length];
            int i = 0;
            foreach (int item in integers)
            {
                arr[i++] = item.ToString();
            }
            return arr;
        }

        public string IntArrayToCommaSeparatedString(int[] integers)
        {
            StringBuilder sb = new StringBuilder(integers.Length);
            for (int i = 0; i <= integers.Length; i++)
            {
                sb.Append(integers[i]);
                if (i <= integers.Length)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        public string StringArrayToCommaSeparatedString(string[] strings)
        {
            return string.Join(",", strings);
        }

        public string[] ObjectsArrayToStringArray(object[] objects)
        {
            string[] strings = new string[objects.Length];
            int i = 0;
            foreach (object o in objects)
            {
                strings[i] = o.ToString();
                i++;
            }
            return strings;
        }

        public string ObjectsArrayToCommaSeparatedString(object[] objects)
        {
            return this.StringArrayToCommaSeparatedString(this.ObjectsArrayToStringArray(objects));
        }

        public EntityBuilder()
        { }

        public object[] MapCollectionTo(Type obj, XmlNodeList list, string path = "")
        {
            return null;
        }

        /// <summary>
        /// Метод предназначен для того, чтобы в автоматическом, или полуавтоматическом режиме присваивать значение полям
        /// экземпляров объектов, предварительно создавая эти экземпляры
        /// </summary>
        /// <param name="obj">Тип объекта, к которому следует привести вновь созданный экземпляр</param>
        /// <param name="node">Ветка xml, в которой содержатся данные, которыми необходимо заполнить экземпляр</param>
        /// <returns>Объект указанного типа</returns>
        public object MapTo(Type obj, XmlNode node, string path = "")
        {
            object instance = null;
            // получаем метаданные для класса
            ClassMetadata classMetadata = (ClassMetadata)System.Attribute.GetCustomAttribute(obj, typeof(ClassMetadata));
            // если метаданные существуют, и в переданной ноде существует нужная нам ветка
            if (classMetadata != null && node.SelectSingleNode(classMetadata.GetNodeName()) != null)
            {
                FieldInfo[] fields = obj.GetFields();       // получаем список полей переданного нам типа,
                instance = Activator.CreateInstance(obj);   // создаем экземпляр переданного типа
                foreach (FieldInfo ff in fields)             
                {
                    // получаем метаданные конкретного поля
                    MapperInfo meta = (MapperInfo)System.Attribute.GetCustomAttribute(ff, typeof(MapperInfo));
                    if (meta != null)
                    {
                        // Если Принудительно не указано, к какому типу нужно приводить значение свойства,
                        // Пробуем стандартные типы
                        XmlNode fieldNode = node.SelectSingleNode(classMetadata.GetNodeName() + "/" + meta.GetNodeName());
                        
                        string val = "";
                        if(fieldNode.HasChildNodes && fieldNode.FirstChild.NodeType == XmlNodeType.Element)
                        {

                        }
                        else if(fieldNode.HasChildNodes && fieldNode.FirstChild.NodeType == XmlNodeType.Text)
                        {
                            val = fieldNode.InnerText;
                        }

                        if (meta.CastTo == null)
                        {
                            System.Windows.Forms.MessageBox.Show(ff.Name + " = " + ff.FieldType.ToString());
                            if (fieldNode != null)
                            {
                                ff.SetValue(instance, this.getKnownValue(ff, val));
                            }
                        }
                        else // в противном случае пляшем :D
                        {
                            ff.SetValue(instance, Convert.ChangeType(val, meta.CastTo));
                        }
                    }
                }
            }
                
            return instance;
        }

        private object getKnownValue(FieldInfo field, object value)
        {
            switch(field.FieldType.ToString())
            {
                case "System.DateTime":
                    {
                        return this.FromUnixTime(Convert.ToInt32(value));//field.SetValue(instance, this.FromUnixTime(Convert.ToInt32(val)));
                        break;
                    }
                case "ApiCore.Messages.MessageState":
                    {
                        return Convert.ToInt32(value);
                        break;
                    }
                default:
                    {
                        return Convert.ChangeType(value, field.FieldType);
                        break;
                    }
            }

            throw new Exception(string.Format("Can`t determine field {0} type with value {1}", field.Name, value.ToString()));
        }
    }
}
