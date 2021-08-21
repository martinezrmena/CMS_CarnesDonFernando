using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApi_CarnesDonFernando.Helpers
{
    public static class ExtensionXMLDes
    {
        //This method is to handle if element is missing
        public static string ElementValueNull(this XElement element)
        {
            if (element != null)
                return element.Value;

            return "";
        }

        //This method is to handle if attribute is missing
        public static string AttributeValueNull(this XElement element, string attributeName)
        {
            if (element == null)
                return "";
            else
            {
                XAttribute attr = element.Attribute(attributeName);
                return attr == null ? "" : attr.Value;
            }
        }
    }
}
