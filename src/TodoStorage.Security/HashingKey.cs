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
    using TodoStorage.Domain;

    public class HashingKey : CollectionKey
    {
        private readonly byte[] secret;

        internal HashingKey(Guid identifier, byte[] secret)
            : base(identifier)
        {
            Guard.EnsureNotNull(secret, nameof(secret));
            if (secret.Length == 0)
            {
                throw new ArgumentException(null, nameof(secret));
            }

            this.secret = secret;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherHashingKey = obj as HashingKey;
            return Identifier.Equals(otherHashingKey.Identifier)
                && secret.SequenceEqual(otherHashingKey.secret);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (17 * 486187739) + Identifier.GetHashCode();
                foreach (var b in secret)
                {
                    hash = (hash * 486187739) + b;
                }

                return hash;
            }
        }
    }
}
