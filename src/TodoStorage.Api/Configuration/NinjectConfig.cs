/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016-2017  Simon Wendel
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TodoStorage.Api.Configuration.NinjectConfig), "Stop")]

namespace TodoStorage.Api.Configuration
{
    using System;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Http.Dispatcher;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Web.Common;
    using Ploeh.Hyprlinkr;

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

        internal static IKernel CreateKernel()
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
                    .From("TodoStorage.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .Join
                    .From("TodoStorage.Persistence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .Join
                    .From("TodoStorage.Security, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").IncludingNonePublicTypes().SelectAllClasses()
                    .BindDefaultInterfaces();
            });

            kernel.Bind<HashAlgorithm>().To<HMACSHA256>();

            kernel
                .Bind<IHttpControllerActivator>()
                .To<DefaultHttpControllerActivator>()
                .Intercept()
                .With<ControllerActivatorLogInterceptor>();

            // a bit convoluted way of injecting a routelinker, but this is the most 
            // sane way I could find of deferring the creation of the RouteLinker until 
            // a request would be properly initialized
            kernel
                .Bind<Func<HttpRequestMessage, IResourceLinker>>()
                .ToMethod(context => (request => new RouteLinker(request)));
        }        
    }
}
