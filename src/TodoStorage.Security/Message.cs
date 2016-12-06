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

namespace TodoStorage.Security
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using SimonWendel.GuardStatements;

    public class Message
    {
        private readonly Guid appId;

        private readonly string method;

        private readonly Uri uri;

        private readonly ulong timestamp;

        private readonly string nonce;

        private readonly string body;

        private readonly IReadOnlyList<byte> signature;

        public Message(Guid appId, string method, Uri uri, ulong timestamp, string nonce, string body, byte[] signature)
        {
            Guard.EnsureNonempty(appId);
            Guard.EnsureNotNull(method);
            Guard.EnsureNotNull(uri);
            Guard.EnsureNotNull(nonce);
            Guard.EnsureNotNull(body);
            Guard.EnsureNotNull(signature);

            this.appId = appId;
            this.method = method;
            this.uri = uri;
            this.timestamp = timestamp;
            this.nonce = nonce;
            this.body = body;
            this.signature = new List<byte>(signature).AsReadOnly();
        }

        public Guid AppId => appId;

        public string Method => method;

        public Uri Uri => uri;

        public ulong Timestamp => timestamp;

        public string Nonce => nonce;

        public string Body => body;

        public IReadOnlyList<byte> Signature => signature;

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}:{2}:{3}:{4}:{5}",
                appId, 
                method, 
                uri, 
                timestamp, 
                nonce, 
                body);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherMessage = obj as Message;
            return appId.Equals(otherMessage.appId)
                && method.Equals(otherMessage.method)
                && uri.Equals(otherMessage.uri)
                && timestamp.Equals(otherMessage.timestamp)
                && nonce.Equals(otherMessage.nonce)
                && body.Equals(otherMessage.body)
                && signature.SequenceEqual(otherMessage.signature);
        }

        public override int GetHashCode()
        {
            var start = 17;
            var multiplier = 486187739;

            unchecked
            {
                int hash = start;
                hash = (hash * multiplier) + appId.GetHashCode();
                hash = (hash * multiplier) + method.GetHashCode();
                hash = (hash * multiplier) + uri.GetHashCode();
                hash = (hash * multiplier) + timestamp.GetHashCode();
                hash = (hash * multiplier) + nonce.GetHashCode();
                hash = (hash * multiplier) + body.GetHashCode();
                foreach (var b in signature)
                {
                    hash = (hash * multiplier) + b;
                }

                return hash;
            }
        }
    }
}
