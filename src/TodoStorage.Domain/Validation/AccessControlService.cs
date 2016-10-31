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

namespace TodoStorage.Domain.Validation
{
    using Data;
    using SimonWendel.GuardStatements;

    internal class AccessControlService : IAccessControlService
    {
        private readonly IAccessControlRepository repository;

        public AccessControlService(IAccessControlRepository repository)
        {
            Guard.EnsureNotNull(repository, nameof(repository));

            this.repository = repository;
        }

        public bool IsOwnerOf(CollectionKey ownerKey, Todo todo)
        {
            Guard.EnsureNotNull(ownerKey, nameof(ownerKey));

            if (todo.Id.HasValue)
            {
                return repository.IsOwnerOf(ownerKey, todo.Id.Value);
            }

            return false;
        }
    }
}
