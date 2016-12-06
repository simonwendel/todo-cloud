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
    using System.Linq;
    using SimonWendel.GuardStatements;

    internal class HashingKey : IHashingKey
    {
        private readonly IMessageHasher hasher;

        private readonly Guid identifier;

        private readonly byte[] secret;

        public HashingKey(IMessageHasher hasher, Guid identifier, byte[] secret)
        {
            Guard.EnsureNotNull(hasher, nameof(hasher));
            Guard.EnsureNonempty(identifier, nameof(identifier));
            Guard.EnsureNotNull(secret, nameof(secret));
            if (secret.Length == 0)
            {
                throw new ArgumentException(null, nameof(secret));
            }

            this.hasher = hasher;
            this.identifier = identifier;
            this.secret = secret;
        }

        public bool Verify(Message message)
        {
            Guard.EnsureNotNull(message, nameof(message));

            var rehash = hasher.HashMessage(message.ToString());
            return message.Signature.SequenceEqual(rehash);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherHashingKey = obj as HashingKey;
            return identifier.Equals(otherHashingKey.identifier)
                && secret.SequenceEqual(otherHashingKey.secret);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (17 * 486187739) + identifier.GetHashCode();
                foreach (var b in secret)
                {
                    hash = (hash * 486187739) + b;
                }

                return hash;
            }
        }
    }
}
