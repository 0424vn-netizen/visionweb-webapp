using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Common
{
    public class NinjectSetting : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string TypeName
        {
            get
            {
                return this["type"].ToString();
            }
        }
        [ConfigurationProperty("to", IsRequired = true)]
        public string BindTo
        {
            get
            {
                return this["to"].ToString();
            }
        }
        [ConfigurationProperty("module")]
        public string Module
        {
            get
            {
                return this["module"].ToString();
            }
        }
    }
    [ConfigurationCollection(typeof(NinjectSetting), AddItemName = "bind", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class NinjectBindingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NinjectSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            return ((NinjectSetting)element).TypeName;
        }
    }
    public class NinjectConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("bindings")]
        public NinjectBindingCollection Settings
        {
            get
            {
                return (NinjectBindingCollection)this["bindings"];
            }
        }
    }
    public abstract class ASNinjectModule : NinjectModule
    {
        readonly IDictionary<string, string> _bindings = null;

        protected ASNinjectModule(string moduleName)
        {
            _bindings = new Dictionary<string, string>();
            NinjectConfiguration config = (NinjectConfiguration)ConfigurationManager.GetSection("ninjectSection");
            foreach (NinjectSetting n in config.Settings)
            {
                if (string.IsNullOrEmpty(n.Module) || n.Module == moduleName) _bindings.Add(n.TypeName, n.BindTo);
            }
        }
        public override void Load()
        {
            if (_bindings != null)
            {
                foreach (var b in _bindings)
                {
                    Bind(Type.GetType(b.Key)).To(Type.GetType(b.Value));
                }
            }
        }

    }
}
