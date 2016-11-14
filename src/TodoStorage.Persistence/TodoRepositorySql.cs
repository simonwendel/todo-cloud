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
    /// <summary>
    /// Constants holding SQL statements for use by the <see cref="TodoRepository"/> class.
    /// </summary>
    internal static class TodoRepositorySql
    {
        /// <summary>
        /// Selects all todo items belonging to one storage key.
        /// </summary>
        public const string SelectMany = @"
SELECT
    [Id],
    [AppId],
    [Title],
    [Description],
    [Created],
    [Recurring],
    [NextOccurrence],
    [ColorValue]
FROM
    [Todo]
WHERE
    [AppId] = @AppId
ORDER BY
    [Id]";

        /// <summary>
        /// Adds a new todo item to the repository.
        /// </summary>
        public const string Add = @"
INSERT INTO
    [Todo] (
        [AppId],
        [Title],
        [Description],
        [Created],
        [Recurring],
        [NextOccurrence],
        [ColorValue]
    )
VALUES (
    @AppId,
    @Title,
    @Description,
    @Created,
    @Recurring,
    @NextOccurrence,
    @ColorValue
);
SELECT CAST(SCOPE_IDENTITY() as INT)";

        /// <summary>
        /// Updates a todo item by persisting all properties, except for Id and AppId.
        /// </summary>
        public const string Update = @"
UPDATE [Todo]
SET
    [Title] = @Title,
    [Description] = @Description,
    [Created] = @Created,
    [Recurring] = @Recurring,
    [NextOccurrence] = @NextOccurrence,
    [ColorValue] = @ColorValue
WHERE
    [Id] = @Id";

        /// <summary>
        /// Deletes any todo item with supplied Id.
        /// </summary>
        public const string Delete = @"
DELETE FROM
    [Todo]
WHERE
    [Id] = @Id";
    }
}
