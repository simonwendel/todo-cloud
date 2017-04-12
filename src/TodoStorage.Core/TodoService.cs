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

namespace TodoStorage.Core
{
    using System.Collections.Generic;
    using SimonWendel.GuardStatements;

    internal class TodoService : ITodoService
    {
        private readonly IAccessControlService accessControlService;

        private readonly ITodoRepository todoRepository;

        public TodoService(IAccessControlService accessControlService, ITodoRepository todoRepository)
        {
            Guard.EnsureNotNull(accessControlService, nameof(accessControlService));
            Guard.EnsureNotNull(todoRepository, nameof(todoRepository));

            this.accessControlService = accessControlService;
            this.todoRepository = todoRepository;
        }

        public IList<Todo> GetAll(CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            return todoRepository.GetAll(collectionKey);
        }

        public Todo Add(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            todo.Id = todoRepository.Add(todo, collectionKey);
            return todo;
        }

        public void Update(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            if (accessControlService.IsOwnerOf(collectionKey, todo) == false)
            {
                throw new AccessControlException();
            }

            var didUpdate = todoRepository.Update(todo);
            if (didUpdate == false)
            {
                throw new UpdateFailedException();
            }
        }

        public void Delete(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            if (accessControlService.IsOwnerOf(collectionKey, todo) == false)
            {
                throw new AccessControlException();
            }

            var didDelete = todoRepository.Delete(todo);
            if (didDelete == false)
            {
                throw new DeleteFailedException();
            }
        }
    }
}
