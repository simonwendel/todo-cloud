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
    using System.Linq;
    using SimonWendel.GuardStatements;

    public class Message
    {
        private readonly Guid appId;

        private readonly string body;

        private readonly IReadOnlyList<byte> signature;

        public Message(Guid appId, string body, byte[] signature)
        {
            Guard.EnsureNonempty(appId);
            Guard.EnsureNotNull(body);
            Guard.EnsureNotNull(signature);

            this.appId = appId;
            this.body = body;
            this.signature = new List<byte>(signature).AsReadOnly();
        }

        public Guid AppId => appId;

        public string Body => body;

        public IReadOnlyList<byte> Signature => signature;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherMessage = obj as Message;
            return appId.Equals(otherMessage.appId)
                && body.Equals(otherMessage.body)
                && signature.SequenceEqual(otherMessage.signature);
        }
    }
}
