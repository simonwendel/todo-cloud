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
    using Domain.Data;
    using SimonWendel.GuardStatements;

    internal class TodoRepository : ITodoRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public TodoRepository(IDbConnectionFactory connectionFactory)
        {
            Guard.EnsureNotNull(connectionFactory, nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public IList<Todo> GetTodo(CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var whereConstraint = new { StorageKey = collectionKey.Identifier };
                return connection
                    .Query<Todo, Color, Todo>(
                        TodoSql.SelectMany,
                        AttachColorTypeObjectToTodo,
                        whereConstraint,
                        splitOn: "ColorName")
                    .ToList();
            }
        }

        public bool Delete(int id)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var rowsAffected = connection.Execute(TodoSql.Delete, new { Id = id });
                return rowsAffected != 0;
            }
        }

        public Todo Add(Todo todo, CollectionKey collectionKey)
        {
            Guard.EnsureNotNull(todo, nameof(todo));
            Guard.EnsureNotNull(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var insertedId = connection.Query<int>(
                    TodoSql.Add,
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

                todo.Id = insertedId;
                return todo;
            }
        }

        public bool Update(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            using (var connection = connectionFactory.GetConnection())
            {
                var rowsAffected = connection.Execute(
                    TodoSql.Update,
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

        private static Todo AttachColorTypeObjectToTodo(Todo todo, Color color)
        {
            todo.Color = color;
            return todo;
        }

        private static class TodoSql
        {
            public const string SelectMany = @"
SELECT
    [Id],
    [StorageKey],
    [Title],
    [Description],
    [Created],
    [Recurring],
    [NextOccurrence],
    [ColorName],
    [ColorValue]
FROM
    [TodoItem]
WHERE
    [StorageKey] = @StorageKey
ORDER BY
    [Id]";

            public const string Add = @"
INSERT INTO
    [TodoItem] (
        [StorageKey],
        [Title],
        [Description],
        [Created],
        [Recurring],
        [NextOccurrence],
        [ColorName],
        [ColorValue]
    )
VALUES (
    @StorageKey,
    @Title,
    @Description,
    @Created,
    @Recurring,
    @NextOccurrence,
    @ColorName,
    @ColorValue
);
SELECT CAST(SCOPE_IDENTITY() as INT)";

            public const string Update = @"
UPDATE [TodoItem]
SET
    [Title] = @Title,
    [Description] = @Description,
    [Created] = @Created,
    [Recurring] = @Recurring,
    [NextOccurrence] = @NextOccurrence,
    [ColorName] = @ColorName,
    [ColorValue] = @ColorValue
WHERE
    [Id] = @Id";

            public const string Delete = @"
DELETE FROM
    [TodoItem]
WHERE
    [Id] = @Id";
        }
    }
}
