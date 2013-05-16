using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using System.Xml;

namespace ApiCore.Utils.Mapper
{
    public class XmlResponseMapper
    {
        public object Map(XmlNode response, Type mapToClass)
        {
            // создали экземпляр
            object item = Activator.CreateInstance(mapToClass);
            Type t = mapToClass;
            // Получили метадату с описанием класса
            ClassMetadata[] attrs = (ClassMetadata[])t.GetCustomAttributes(typeof(ClassMetadata), true);

            // получили список полей класса
            FieldInfo[] fields = t.GetFields();

            // Пробегаемся по каждому полю для того, чтобы получить его атрибуты, и соответствующим образом их обработать
            foreach (FieldInfo f in fields)
            {
                // Получаем атрибуты этого поля
                object[] attribs =  f.GetCustomAttributes(typeof (MapperInfo), true);
                if (attribs.Length == 0)
                    continue;
                MapperInfo map = (MapperInfo) attribs[0];
                

                // Получили экземпляр поля
                FieldInfo fi = item.GetType().GetField(f.Name);
                try
                {
                    if (map.CastTo == typeof (int))
                    {
                        fi.SetValue(item, Convert.ToInt32(response.SelectSingleNode(map.GetNodeName()).InnerText));
                    }
                    else
                    {
                        fi.SetValue(item, response.SelectSingleNode(map.GetNodeName()).InnerText);
                    }
                }catch(Exception e)
                {
                }
            }
            return null;
        }
    }
}
