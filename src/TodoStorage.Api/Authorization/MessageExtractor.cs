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

namespace TodoStorage.Api.Authorization
{
    using System;
    using System.Globalization;
    using System.Web.Http.Controllers;
    using SimonWendel.GuardStatements;
    using TodoStorage.Security;

    internal class MessageExtractor : IMessageExtractor
    {
        private readonly IMessageFactory messageFactory;

        public MessageExtractor(IMessageFactory messageFactory)
        {
            Guard.EnsureNotNull(messageFactory, nameof(messageFactory));

            this.messageFactory = messageFactory;
        }

        public IMessage ExtractMessage(HttpActionContext context)
        {
            Guard.EnsureNotNull(context, nameof(context));

            var authHeader = context.Request.Headers.Authorization;
            var authScheme = authHeader.Scheme;

            Guard.EnsureThat(authScheme.Equals(ValidAuthenticationSchemes.Hmac), nameof(context));

            var authParameter = context.Request.Headers.Authorization.Parameter;
            var fields = authParameter.Split(':');

            try
            {
                var appId = new Guid(fields[0]);
                var signature = fields[1];
                var nonce = fields[2];
                var timestamp = Convert.ToUInt64(fields[3], CultureInfo.InvariantCulture);

                var method = context.Request.Method.Method;
                var uri = context.Request.RequestUri;

                // no point in turning the whole method into async because of this
                // we're not waiting on anything else
                string content;
                using (var contentTask = context.Request.Content.ReadAsStringAsync())
                {
                    contentTask.Wait();
                    content = contentTask.Result;
                }

                return messageFactory.Build(
                    appId, method, uri, timestamp, nonce, content, signature);
            }
            catch (Exception ex)
            {
                throw new BadMessageFormatException("Ill-formed message", ex);
            }
        }
    }
}
