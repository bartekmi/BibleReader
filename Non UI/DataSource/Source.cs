using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BibleReader.DataSource
{
    public abstract class Source
    {
        public static string GetAttribute(XmlNode node, string attributeName, string defaultValue = "")
        {
            if (node.Attributes == null)
                return defaultValue;
            XmlAttribute attribute = node.Attributes[attributeName];
            return attribute == null ? defaultValue : attribute.Value;
        }

        public static int GetAttributeInt(XmlNode node, string attributeName)
        {
            string attribute = GetAttribute(node, attributeName, null);

            int i = 0;
            if (attribute != null)
                int.TryParse(attribute, out i);

            return i;
        }
    }
}
