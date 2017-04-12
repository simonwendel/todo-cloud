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

namespace TodoStorage.Security
{
    using System;
    using SimonWendel.GuardStatements;

    internal class MessageFactory : IMessageFactory
    {
        private readonly IHashConverter hashConverter;

        public MessageFactory(IHashConverter hashConverter)
        {
            Guard.EnsureNotNull(hashConverter, nameof(hashConverter));

            this.hashConverter = hashConverter;
        }

        public IMessage Build(Guid appId, string method, Uri uri, ulong timestamp, string nonce, string body, string signature)
        {
            Guard.EnsureNotNull(method, nameof(method));
            Guard.EnsureNotNull(uri, nameof(uri));
            Guard.EnsureNotNull(nonce, nameof(nonce));
            Guard.EnsureNotNull(body, nameof(body));
            Guard.EnsureNotNull(signature, nameof(signature));

            var signatureBytes = hashConverter.HexToBytes(signature);

            return new Message(appId, method, uri.PathAndQuery, timestamp, nonce, body, signatureBytes);
        }
    }
}
