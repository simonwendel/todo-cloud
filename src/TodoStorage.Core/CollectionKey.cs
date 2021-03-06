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

namespace TodoStorage.Core
{
    using System;
    using SimonWendel.GuardStatements;

    public class CollectionKey
    {
        private readonly Guid identifier;
        
        public CollectionKey(Guid identifier)
        {
            Guard.EnsureNonempty(identifier, nameof(identifier));

            this.identifier = identifier;
        }

        public Guid Identifier => identifier;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherCollectionKey = obj as CollectionKey;
            return Identifier.Equals(otherCollectionKey.Identifier);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (17 * 486187739) + Identifier.GetHashCode();
            }
        }
    }
}
