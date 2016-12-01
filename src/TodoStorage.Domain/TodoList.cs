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
    using System.Linq;
    using SimonWendel.GuardStatements;

    internal class TodoList : ITodoList
    {
        private readonly ITodoService todoService;

        private readonly CollectionKey key;

        private List<Todo> items;

        public TodoList(ITodoService todoService, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todoService, nameof(todoService));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            key = collectionKey;
            this.todoService = todoService;

            RefreshList();
        }

        public IReadOnlyList<Todo> Items => items.AsReadOnly();

        internal CollectionKey Key => key;

        public void Add(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            todoService.Add(todo, Key);
            RefreshList();
        }
        
        public void Update(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            todoService.Update(todo, Key);
            RefreshList();
        }

        public void Delete(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            todoService.Delete(todo, Key);
            RefreshList();
        }

        #region System.Object overrides

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherTodoList = obj as TodoList;
            return Key.Equals(otherTodoList.Key)
                && Items.SequenceEqual(otherTodoList.Items);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (17 * 486187739) + Key.GetHashCode();
                foreach (var todoItem in Items)
                {
                    hash = (hash * 486187739) + todoItem.GetHashCode();
                }

                return hash;
            }
        }

        #endregion

        private void RefreshList()
        {
            items = todoService.GetAll(key).ToList();
        }
    }
}
