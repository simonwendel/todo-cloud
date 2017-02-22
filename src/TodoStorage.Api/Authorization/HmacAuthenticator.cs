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
    using System.Web.Http.Filters;
    using SimonWendel.GuardStatements;
    using TodoStorage.Security;

    internal class HmacAuthenticator : IAuthenticator
    {
        private readonly IHashingKeyFactory keyFactory;

        private readonly IMessageExtractor messageExtractor;

        public HmacAuthenticator(IHashingKeyFactory keyFactory, IMessageExtractor messageExtractor)
        {
            Guard.EnsureNotNull(keyFactory, nameof(keyFactory));
            Guard.EnsureNotNull(messageExtractor, nameof(messageExtractor));

            this.keyFactory = keyFactory;
            this.messageExtractor = messageExtractor;
        }

        public void Authenticate(HttpAuthenticationContext context)
        {
            Guard.EnsureNotNull(context, nameof(context));

            var request = context.Request;
            var authorization = request.Headers.Authorization;
            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != ValidAuthenticationSchemes.Hmac)
            {
                return;
            }

            IMessage message;
            try
            {
                message = messageExtractor.ExtractMessage(context.ActionContext);
            }
            catch (BadMessageFormatException)
            {
                context.Principal = null;
                context.ErrorResult = new AuthenticationFailureResult("Bad message", context.Request);
                return;
            }

            IHashingKey key;
            try
            {
                key = keyFactory.Build(message.AppId);
            }
            catch (KeyNotFoundException)
            {
                context.Principal = null;
                context.ErrorResult = new AuthenticationFailureResult("Invalid key", context.Request);
                return;
            }

            if (key.Verify(message) == false)
            {
                context.Principal = null;
                context.ErrorResult = new AuthenticationFailureResult("Invalid signature", context.Request);
                return;
            }

            context.Principal = new SignedMessagePrincipal(message);
            return;
        }
    }
}
