using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

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
                FieldInfo[] fieldsCollection = obj.GetFields();       // получаем список полей переданного нам типа,
                instance = Activator.CreateInstance(obj);   // создаем экземпляр переданного типа
                foreach (FieldInfo field in fieldsCollection)             
                {
                    // получаем метаданные конкретного поля
                    Metadata meta = (Metadata)System.Attribute.GetCustomAttribute(field, typeof(Metadata));
                    if (meta != null)
                    {
                        // Если Принудительно не указано, к какому типу нужно приводить значение свойства,
                        // Пробуем стандартные типы
                        XmlNode fieldNode = node.SelectSingleNode(classMetadata.GetNodeName() + "/" + meta.GetNodeName());
                        string val = fieldNode.InnerText;
                        
                        if (meta.CastTo == null)
                        {
                            System.Windows.Forms.MessageBox.Show(field.Name + " = " + field.FieldType.ToString());
                            if (fieldNode != null)
                            {
                                field.SetValue(instance, this.getKnownValue(field, val));
                            }
                        }
                        else // в противном случае пляшем :D
                        {
                            field.SetValue(instance, Convert.ChangeType(val, meta.CastTo));
                        }
                    }
                }
            }
                
            return instance;
        }

        private object getKnownValue(FieldInfo field, object value)
        {
            //if (field.FieldType == typeof(System.Int32)) // int
            //{
            //    return Convert.ToInt32(value);//f.SetValue(instance, (meta.IsCollection ? this.MapCollection(typeof(f.FieldType), fieldNode.SelectNodes(meta.GetNodeName()), "") : Convert.ToInt32(val)));
            //}
            //if (field.FieldType == typeof(System.String)) // строка
            //{
            //    return value;//field.SetValue(instance, val);
            //}
            //if (field.FieldType == typeof(System.Boolean)) // да не
            //{
            //    return Convert.ToBoolean(value);//field.SetValue(instance, Convert.ToBoolean(val));
            //}

            switch(field.FieldType.ToString())
            {
                case "System.DateTime":
                    {
                        return this.FromUnixTime(Convert.ToInt32(value));//field.SetValue(instance, this.FromUnixTime(Convert.ToInt32(val)));
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
