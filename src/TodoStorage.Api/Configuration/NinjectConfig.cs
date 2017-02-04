[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Stop")]

namespace TodoStorage.Api.Configuration
{
    using System;
    using System.Security.Cryptography;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using TodoStorage.Api.Authorization;

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
                    .Join
                    .From("TodoStorage.Security, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .BindDefaultInterfaces();
            });

            kernel.Bind<HashAlgorithm>().To<HMACSHA256>();
            kernel.Bind<IAuthenticator>().To<HmacAuthenticator>();
            kernel.Bind<IChallenger>().To<HmacChallenger>();
        }        
    }
}
