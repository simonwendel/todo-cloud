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

    public interface IHashingKeyFactory
    {
        /// <summary>
        /// Builds a hashing key object, with secret fetched from main storage.
        /// </summary>
        /// <param name="appId">Identifier for the calling app.</param>
        /// <returns>A hashing key object.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when app ID is not present in storage.
        /// </exception>
        IHashingKey Build(Guid appId);
    }
}
