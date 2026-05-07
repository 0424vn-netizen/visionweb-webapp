using Ninject;
using Ninject.Modules;

namespace AS.VW.Common
{
    public static class DependencyManager
    {       
        public static IKernel GetKernel(NinjectModule module)
        {
            IKernel kernel = new StandardKernel(module);
            return kernel;
        }
    }
}
