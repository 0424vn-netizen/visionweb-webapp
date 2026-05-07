using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using AS.Common;

namespace AS.WS.Entities
{
    /// <summary>
    /// Summary description for rm_MCF_ColumnSetting
    /// </summary>
    public class CustomizeColumn
    {
        public string Key { get; set; }
        public string ASFormat { get; set; }
        public string CustomFormat { get; set; }
        public string ReSourceKey { get; set; }
        public bool IsDefault { get; set; }
        public int Width { get; set; }
        public int OrderNo { get; set; }
        public string DefaultValue { get; set; }
    }

    public class BarometerColumn
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("BgColor")]
        public string BgColor { get; set; }

        [XmlAttribute("Color")]
        public string Color { get; set; }
    }

    [Serializable]
    [XmlRoot("Config")]
    public class BarometerConfig
    {
        [XmlElement("Column")]
        public List<BarometerColumn> Configs { get; set; }
    }
}
