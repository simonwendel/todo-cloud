/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016  Simon Wendel
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

namespace TodoStorage.Api.Configuration
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using Newtonsoft.Json.Serialization;
    using TodoStorage.Api.Authorization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var logFilter = config.DependencyResolver.GetService(typeof(LogExceptionFilterAttribute)) as IFilter;
            if (logFilter == null)
            {
                throw new InvalidOperationException();
            }

            config.Filters.Add(logFilter);

            var authenticationFilter = config.DependencyResolver.GetService(typeof(DefaultAuthenticationFilter)) as IFilter;
            if(authenticationFilter == null)
            {
                throw new InvalidOperationException();
            }

            config.Filters.Add(authenticationFilter);

            config.SuppressHostPrincipal();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}
