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

namespace TodoStorage.Domain
{
    using System.Collections.Generic;
    using SimonWendel.GuardStatements;

    internal class TodoListFactory : ITodoListFactory
    {
        public TodoList Create(CollectionKey collectionKey, IEnumerable<Todo> todoItems)
        {
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));
            Guard.EnsureNotNull(todoItems, nameof(todoItems));

            return new TodoList(collectionKey, todoItems);
        }
    }
}
