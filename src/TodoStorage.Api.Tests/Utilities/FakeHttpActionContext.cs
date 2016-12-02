﻿/*
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

namespace TodoStorage.Api.Tests.Utilities
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Fakes the <see cref="HttpActionContext"/> class for use in unit testing of 
    /// anything needing such a fake.
    /// </summary>
    internal class FakeHttpActionContext : HttpActionContext
    {
        public FakeHttpActionContext()
            : base()
        {
            var request = new HttpRequestMessage();

            var controllerContext = new HttpControllerContext();
            controllerContext.Request = request;

            var headers = request.Headers;
            headers.Authorization = new AuthenticationHeaderValue("test");

            ControllerContext = controllerContext;
        }
    }
}