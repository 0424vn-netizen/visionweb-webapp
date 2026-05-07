using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.IO;

namespace AS.Web.Business
{
    public static class ObjectsConverter
    {
        /// <summary>
        /// Convert entity array to datatable        
        /// </summary>
        /// <param name="objectArray"></param>
        /// <returns></returns>
        public static DataTable ToDataSet(Object[] objectArray)
        {
            DataSet ds = new DataSet();
            XmlSerializer xmlSerializer = new XmlSerializer(objectArray.GetType());
            StringWriter writer = new StringWriter();
            xmlSerializer.Serialize(writer, objectArray);
            StringReader reader = new StringReader(writer.ToString());
            ds.ReadXml(reader);
            if (ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
        
    }
}
