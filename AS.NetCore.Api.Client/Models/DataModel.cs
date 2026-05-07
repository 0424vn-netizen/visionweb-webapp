using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace AS.NetCore.Api.Client.Models
{
    /// <summary>
    /// Autocast model
    /// </summary>
    public abstract class DataModel
    {
        private sealed class TimestampObject
        {
            public string Timestamp { get; set; } = string.Empty;
        }

        private static readonly object NotExistProperty = new object();        

        private static string ToCamelCase(string s)
        {
            return char.ToLower(s[0]) + s.Substring(1).Replace("_", "");
        }
        private static object GetValue(object data, string name)
        {
            if (string.IsNullOrEmpty(name)) return data;
            if (name.Contains("."))
            {
                string rName = name.Substring(0, name.IndexOf("."));
                name = name.Replace(rName + ".", "");
                data = GetValue(data, rName);
                return GetValue(data, name);
            }

            var dictionaryData = data as IDictionary<string, object>;
            if (dictionaryData != null)
            {
                var nameCamel = ToCamelCase(name);
                var dicData = (IDictionary<string, object>)data;
                if (dicData.ContainsKey(nameCamel)) 
                    return dicData[nameCamel];                
            }

            var jsonObject = data as JObject;
            if (jsonObject != null)
            {
                var jsonData = (JObject)data;
                var nameCamel = ToCamelCase(name);
                Dictionary<string, object> newJsonData = new Dictionary<string, object>(jsonData.ToObject<Dictionary<string, object>>(), StringComparer.OrdinalIgnoreCase);
                if (newJsonData.ContainsKey(name) || newJsonData.ContainsKey(nameCamel))
                {
                    return GetValueOfJObject(jsonData, name);
                }
            }

            var jsonArray = data as JArray;
            if (jsonArray != null)
            {
                var jArray = (JArray)data;
                foreach (JObject jsonData in jArray.Children<JObject>())
                {
                    var nameCamel = ToCamelCase(name);
                    if (jsonData.ContainsKey(name) || jsonData.ContainsKey(nameCamel))
                    {
                        JToken jtoken = jsonData[name];
                        if (IsTokenTypeNull(jtoken))
                        {
                            return null;
                        }
                        else
                        {
                            return jsonData.GetValue(name, StringComparison.OrdinalIgnoreCase).Value<dynamic>();
                        }
                    }
                }
            }

            PropertyInfo prop = data.GetType().GetProperty(name);
            if (prop != null) 
                return prop.GetValue(data);

            return NotExistProperty;
        }

        private static bool IsTokenTypeNull(JToken jtoken)
        {
            return jtoken != null && jtoken.Type == JTokenType.Null;
        }

        private static object GetValueOfJObject(JObject jsonData, string name)
        {
            JToken jtoken = jsonData[name];
            if (IsTokenTypeNull(jtoken))
            {
                return null;
            }
            else
            {
                if (name.Equals(nameof(TimestampObject.Timestamp), StringComparison.OrdinalIgnoreCase))
                {
                    var results = JsonConvert.DeserializeObject<TimestampObject>(jsonData.ToString());
                    return results.Timestamp;
                }
                else
                {
                    return jsonData.GetValue(name, StringComparison.OrdinalIgnoreCase).Value<dynamic>();
                }
            }
        }

        private static void SetValue(IDictionary<string, object> container, string name, object val)
        {
            if (name.Contains("."))
            {
                string rName = name.Substring(0, name.IndexOf("."));
                name = name.Replace(rName + ".", "");
                IDictionary<string, object> uObj = null;
                if (!container.ContainsKey(rName))
                {
                    dynamic obj = new ExpandoObject();
                    uObj = obj;
                    container.Add(rName, obj);
                }
                else
                {
                    uObj = (IDictionary<string, object>)container[rName];
                }
                SetValue(uObj, name, val);
                return;
            }
            name = ToCamelCase(name);
            if (container.ContainsKey(name)) container[name] = val;
            else container.Add(name, val);
        }
        private void Copy(object data, bool onlyAvailableValues)
        {
            if (data == null) return;

            Type type = this.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (var p in props)
            {
                if (!p.CanWrite)
                {
                    continue;
                }

                string sName = p.Name;
                Dictionary<string, string> extSettings = null;

                object val = GetValue(data, sName);

                //try to assign value
                if (val == null)
                {
                    if (!onlyAvailableValues)
                    {
                        p.SetValue(this, null);
                    }
                    continue;
                }
                else if (val == NotExistProperty)
                {
                    continue;
                }

                Type elType;
                Type c = elType = p.PropertyType.GetElementType();
                if (typeof(DataModel).IsAssignableFrom(p.PropertyType))
                {
                    DataModel obj = (DataModel)Activator.CreateInstance(p.PropertyType);
                    p.SetValue(this, obj);
                    obj.Copy(val);
                }
                else if (p.PropertyType.IsArray && typeof(DataModel).IsAssignableFrom(c))
                {
                    CopyValueOfPropertyTypeIsArray(val, p, elType);
                }
                else if (val is JArray valArray && p.PropertyType.HasElementType
                        && (p.PropertyType.GetElementType().IsValueType
                        || p.PropertyType.GetElementType() == typeof(string)))
                {
                    //copy value for Array of valueType or string
                    CopyValueOfPropertyIsValueTypeOrString(p, valArray);
                }
                else
                {
                    var convertedValue = DataValueHandler.HandleData(val, p, extSettings);
                    p.SetValue(this, convertedValue);
                }
            }
        }

        private void CopyValueOfPropertyTypeIsArray(object val, PropertyInfo p, Type elType)
        {
            switch (val)
            {
                case JArray valArray:
                    {
                        Array objArray = Array.CreateInstance(elType, valArray.Count);
                        p.SetValue(this, objArray);
                        for (int i = 0; i < valArray.Count; i++)
                        {
                            DataModel obj = (DataModel)Activator.CreateInstance(elType);
                            obj.Copy(valArray[i]);
                            objArray.SetValue(obj, i);
                        }

                        break;
                    }

                case Array valArray:
                    {
                        Array objArray = Array.CreateInstance(elType, valArray.Length);
                        p.SetValue(this, objArray);
                        for (int i = 0; i < valArray.Length; i++)
                        {
                            DataModel obj = (DataModel)Activator.CreateInstance(elType);
                            obj.Copy(valArray.GetValue(i));
                            objArray.SetValue(obj, i);
                        }

                        break;
                    }
            }
        }

        private void CopyValueOfPropertyIsValueTypeOrString(PropertyInfo p, JArray valArray)
        {
            var elementTtype = p.PropertyType.GetElementType();
            Array objArray = Array.CreateInstance(elementTtype, valArray.Count);
            p.SetValue(this, objArray);
            for (int i = 0; i < valArray.Count; i++)
            {
                var convertedValue = Convert.ChangeType(valArray[i], elementTtype);
                objArray.SetValue(convertedValue, i);
            }
        }

        /// <summary>
        /// Copy all properties from other object (same name)
        /// </summary>
        /// <param name="data">data source</param>
        public void Copy(object data)
        {
            Copy(data, false);
        }
     
        /// <summary>
        /// Cast this model to dynamic object with same property name to be easier extend more properties
        /// </summary>
        /// <returns>dynamic object</returns>
        public dynamic ToDynamic()
        {
            dynamic obj = new ExpandoObject();
            IDictionary<string, object> uObj = obj;
            Type type = this.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (var p in props)
            {
                if (!p.CanRead)
                {
                    continue;
                }
                object val = p.GetValue(this);
                string sName = p.Name;

                if (val is DataModel model)
                {
                    SetValue(uObj, sName, model.ToDynamic());
                }
                else if (val != null && p.PropertyType.IsArray && typeof(DataModel).IsAssignableFrom(p.PropertyType.GetElementType()))
                {
                    Array valArray = (Array)val;
                    var arrayDynamic = new object[valArray.Length];

                    for (int i = 0; i < valArray.Length; i++)
                    {
                        arrayDynamic[i] = ((DataModel)valArray.GetValue(i)).ToDynamic();
                    }
                    SetValue(uObj, sName, arrayDynamic);
                }
                else
                {
                    SetValue(uObj, sName, val);
                }
            }
            return obj;
        }

        public void LoadFromJson(string json)
        {
            var dynModel = JsonConvert.DeserializeObject(json);
            this.Copy(dynModel, false);
        }
    }

    public class DataListModel<T> : DataModel where T : DataModel
    {
        public T[] Data { get; set; }
        public int Total { get; set; }
    }
}
