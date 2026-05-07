using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AS.NetCore.Api.Client.Models
{
    public interface IDataValueHandler
    {
        object ConvertValue(object rawValue, PropertyInfo prop, Dictionary<string, string> modelConfigs);
    }
    public class DefaultDataValueHandler : IDataValueHandler
    {
        public virtual object ConvertValue(object rawValue, PropertyInfo prop, Dictionary<string, string> modelConfigs)
        {
            var propertyType = prop.PropertyType;

            // If the object is type of nullable then change type to reference-type
            if (Nullable.GetUnderlyingType(propertyType) != null)
                propertyType = Nullable.GetUnderlyingType(propertyType);

            try
            {
                return ChangeType(rawValue, propertyType);
            }
            catch
            {
                //Logger.Info($"WARNING: Could not convert '{ rawValue }' to { propertyType }")
            }
            return null;
        }

        private object ChangeType(object obj, Type type)
        {
            if (IsList(obj))
            {
                object[] objs = ((IEnumerable)obj).Cast<object>().ToArray();
                Type containedType = type.GetElementType();
                return objs.Select(item => Convert.ChangeType(item, containedType)).ToArray();
            }
            return Convert.ChangeType(obj, type);
        }

        private bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList;
        }
    }

    public static class DataValueHandler
    {
        private static IDataValueHandler _handler;

        public static void SetHandler(IDataValueHandler handler)
        {
            _handler = handler;
        }

        public static object HandleData(object rawValue, PropertyInfo prop, Dictionary<string, string> modelConfigs)
        {
            if (_handler == null)
                _handler = new DefaultDataValueHandler();

            return _handler.ConvertValue(rawValue, prop, modelConfigs);
        }
    }
}
