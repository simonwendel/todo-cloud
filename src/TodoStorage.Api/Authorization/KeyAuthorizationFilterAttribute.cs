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

namespace TodoStorage.Api.Authorization
{
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using SimonWendel.GuardStatements;
    using TodoStorage.Security;

    internal sealed class KeyAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        private readonly IHashingKeyFactory keyFactory;

        private readonly IMessageExtractor messageExtractor;

        public KeyAuthorizationFilterAttribute(IHashingKeyFactory keyFactory, IMessageExtractor messageExtractor)
        {
            Guard.EnsureNotNull(keyFactory, nameof(keyFactory));
            Guard.EnsureNotNull(messageExtractor, nameof(messageExtractor));

            this.keyFactory = keyFactory;
            this.messageExtractor = messageExtractor;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Guard.EnsureNotNull(actionContext, nameof(actionContext));

            var message = messageExtractor.ExtractMessage(actionContext);
            var hashingKey = keyFactory.Build(message.AppId);
            if (hashingKey.Verify(message.Body, message.Signature))
            {
                return;
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}
