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
    using Domain.Validation;

    internal class TodoRepository : ITodoRepository
    {
        private const string TodoSelectionSql = @"
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

        private readonly IDbConnectionFactory connectionFactory;

        public TodoRepository(IDbConnectionFactory connectionFactory)
        {
            Guard.NullParameter(connectionFactory, nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public IList<Todo> GetTodo(CollectionKey collectionKey)
        {
            Guard.NullParameter(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var whereConstraint = new { StorageKey = collectionKey.Identifier };
                return connection
                    .Query<Todo, Color, Todo>(
                        TodoSelectionSql,
                        AttachColorTypeObjectToTodo,
                        whereConstraint,
                        splitOn: "ColorName")
                    .ToList();
            }
        }

        private static Todo AttachColorTypeObjectToTodo(Todo todo, Color color)
        {
            todo.Color = color;
            return todo;
        }
    }
}
