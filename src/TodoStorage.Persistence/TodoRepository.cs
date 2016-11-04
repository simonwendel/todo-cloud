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
    using Domain;
    using SimonWendel.GuardStatements;

    internal class TodoRepository : ITodoRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public TodoRepository(IDbConnectionFactory connectionFactory)
        {
            Guard.EnsureNotNull(connectionFactory, nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public IList<Todo> GetAll(CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var whereConstraint = new { StorageKey = collectionKey.Identifier };
                return connection
                    .Query<Todo, Color, Todo>(
                        TodoRepositorySql.SelectMany,
                        AttachColorTypeObjectToTodo,
                        whereConstraint,
                        splitOn: "ColorName")
                    .ToList();
            }
        }

        public int Add(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var insertedId = connection.Query<int>(
                    TodoRepositorySql.Add,
                    new
                    {
                        StorageKey = collectionKey.Identifier,
                        Title = todo.Title,
                        Description = todo.Description,
                        Created = todo.Created,
                        Recurring = todo.Recurring,
                        NextOccurrence = todo.NextOccurrence,
                        ColorName = todo.Color.ColorName,
                        ColorValue = todo.Color.ColorValue
                    }).Single();

                return insertedId;
            }
        }

        public bool Update(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            using (var connection = connectionFactory.GetConnection())
            {
                var rowsAffected = connection.Execute(
                    TodoRepositorySql.Update,
                    new
                    {
                        Id = todo.Id,
                        Title = todo.Title,
                        Description = todo.Description,
                        Created = todo.Created,
                        Recurring = todo.Recurring,
                        NextOccurrence = todo.NextOccurrence,
                        ColorName = todo.Color.ColorName,
                        ColorValue = todo.Color.ColorValue
                    });
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

        private static Todo AttachColorTypeObjectToTodo(Todo todo, Color color)
        {
            todo.Color = color;
            return todo;
        }
    }
}
