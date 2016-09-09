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

namespace TodoStorage.Persistence
{
    using System.Linq;
    using Dapper;
    using Domain;

    internal class TodoListRepository : ITodoListRepository
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

        public TodoListRepository(IDbConnectionFactory connectionFactory)
        {
            Guard.NullParameter(connectionFactory, nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public TodoList GetList(CollectionKey collectionKey)
        {
            Guard.NullParameter(collectionKey, nameof(collectionKey));

            using (var connection = connectionFactory.GetConnection())
            {
                var whereConstraint = new { StorageKey = collectionKey.Identifier };
                var todos = connection.Query<Todo, Color, Todo>(
                    TodoSelectionSql,
                    MapTodoProperties,
                    whereConstraint,
                    splitOn: "ColorName");

                if (todos.Count() == 0)
                {
                    return null;
                }

                return new TodoList(collectionKey, todos);
            }
        }

        private static Todo MapTodoProperties(Todo todo, Color color)
        {
            todo.Color = color;
            return todo;
        }
    }
}