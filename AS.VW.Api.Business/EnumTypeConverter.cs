using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Business
{
    public class EnumTypeConverter : AutoMapper.ITypeConverter<Enum,Enum>
    {
        public Enum Convert(AutoMapper.ResolutionContext context)
        {
            return (Enum)Enum.Parse(context.DestinationType, context.SourceValue.ToString());
        }
    }
}
