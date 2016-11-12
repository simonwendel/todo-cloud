[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Stop")]

namespace TodoStorage.Api.Configuration
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;

    public static class NinjectConfig 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x =>
            {
                x
                    .FromThisAssembly().IncludingNonePublicTypes().SelectAllClasses()
                    .Join
                    .From("TodoStorage.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .Join
                    .From("TodoStorage.Persistence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .BindDefaultInterfaces();
            });
        }        
    }
}
