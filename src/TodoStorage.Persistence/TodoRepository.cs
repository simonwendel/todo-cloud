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

namespace TodoStorage.Persistence
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using SimonWendel.GuardStatements;
    using TodoStorage.Core;

    internal class TodoRepository : ITodoRepository
    {
        private readonly IConnectionFactory connectionFactory;

        public TodoRepository(IConnectionFactory connectionFactory)
        {
            Guard.EnsureNotNull(connectionFactory, nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public IList<Todo> GetAll(CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var where = new { AppId = collectionKey.Identifier };
                var persistedTodo = connection.Query<PersistedTodoModel>(
                        TodoRepositorySql.SelectMany,
                        where);

                return persistedTodo
                    .Select(PersistedTodoModel.Reconstitute)
                    .ToList();
            }
        }

        public int Add(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var persistedTodo = new PersistedTodoModel(todo)
                {
                    AppId = collectionKey.Identifier
                };

                var insertedId = connection.Query<int>(
                    TodoRepositorySql.Add,
                    persistedTodo).Single();

                return insertedId;
            }
        }

        public bool Update(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            using (var connection = connectionFactory.GetConnection())
            {
                var persistedTodo = new PersistedTodoModel(todo);
                var rowsAffected = connection.Execute(
                    TodoRepositorySql.Update,
                    persistedTodo);
                return rowsAffected != 0;
            }
        }

        public bool Delete(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            if (todo.Id.HasValue == false)
            {
                return false;
            }

            using (var connection = connectionFactory.GetConnection())
            {
                var rowsAffected = connection.Execute(TodoRepositorySql.Delete, new { Id = todo.Id.Value });
                return rowsAffected != 0;
            }
        }
    }
}
