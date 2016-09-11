﻿/*
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

namespace TodoStorage.Domain.Validation
{
    using Data;

    internal class AccessControl : IAccessControl
    {
        private readonly IAccessControlRepository repository;

        public AccessControl(IAccessControlRepository repository)
        {
            Guard.NullParameter(repository, nameof(repository));

            this.repository = repository;
        }

        public bool IsOwnerOf(CollectionKey ownerKey, int todoId)
        {
            Guard.NullParameter(ownerKey, nameof(ownerKey));

            return repository.IsOwnerOf(ownerKey, todoId);
        }
    }
}