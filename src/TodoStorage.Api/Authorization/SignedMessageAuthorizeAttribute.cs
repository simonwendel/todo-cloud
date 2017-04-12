﻿/*
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

namespace TodoStorage.Api.Authorization
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using SimonWendel.GuardStatements;

    internal sealed class SignedMessageAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Will authorize whenever a signed message has been verified and 
        /// made available to the request. Contents of message is not examined.
        /// </summary>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            Guard.EnsureNotNull(actionContext, nameof(actionContext));

            return actionContext.RequestContext.Principal is SignedMessagePrincipal;
        }
    }
}
