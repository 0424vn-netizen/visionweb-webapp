using AS.VW.Common;
using Ninject;

namespace AS.VW.Repository
{
    public static class RepositoryFactory
    {
        static IKernel kernel;

        public static T Create<T>()
        {
            if (kernel == null)
            {
                kernel = DependencyManager.GetKernel(RepositoryInjectionModule.Instance);
            }

            return kernel.Get<T>();
        }
    }

    class RepositoryInjectionModule : ASNinjectModule
    {
        //Singleton
        private static RepositoryInjectionModule _Instance = null;
        public static RepositoryInjectionModule Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new RepositoryInjectionModule();
                }
                return _Instance;
            }
        }
        protected RepositoryInjectionModule() : base("RepositoryInjectionModule") { }

    }
}
