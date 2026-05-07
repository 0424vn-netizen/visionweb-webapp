using Ninject;

namespace AS.VW.Common
{
    public class NinjectServiceFactory
    {
        class SimpleNinjectModule : ASNinjectModule
        {
            public SimpleNinjectModule(string module) : base(module) { }
        }
        static IKernel kernel;

        public static T Create<T>(string module)
        {
            if (kernel == null)
            {
                kernel = DependencyManager.GetKernel(new SimpleNinjectModule(module));
            }

            return kernel.Get<T>();
        }
    }
}
