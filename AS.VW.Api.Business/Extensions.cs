using AS.VW.Api.Business.vw_ApiServicesProxy;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AS.VW.Api.Business
{
    public static class Extensions
    {
        public static void InitAutoMapper()
        {
            Mapper.CreateMap(typeof(LoginActions),
                typeof(AS.VW.Api.Model.Security.LoginActions)).ConvertUsing<EnumTypeConverter>();
            Assembly assemblySrc = Assembly.GetAssembly(typeof(ApiServices));
            Assembly assemblyDest = Assembly.GetAssembly(typeof(AS.VW.Api.Model.Security.BaseFilter));
            IEnumerable<Type> typeSrcs = assemblySrc.GetTypes().Where(t => t.Namespace == typeof(ApiServices).Namespace);
            IEnumerable<Type> typeDests = assemblyDest.GetTypes();
            foreach (Type typeSrc in typeSrcs)
            {
                Type typeDest  = typeDests.SingleOrDefault(x => x.Name.Equals(typeSrc.Name, StringComparison.OrdinalIgnoreCase));
                if (typeDest != null)
                {
                    Mapper.CreateMap(typeSrc, typeDest);
                    Mapper.CreateMap(typeDest, typeSrc);
                }
            }
        }

        public static List<T> ConvertListTo<T>(this IEnumerable<object> source)
        {
            List<T> temp = new List<T>();

            if (source != null)
            {
                foreach (object o in source)
                {
                    temp.Add(Mapper.Map<T>(o));
                }
            }

            return temp;
        }

        public static T ConvertTo<T>(this object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}
