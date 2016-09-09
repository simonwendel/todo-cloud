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

namespace TodoStorage.Persistence.Tests.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using Domain;

    internal static class Data
    {
        public static readonly CollectionKey TestCollectionKey = new CollectionKey(Guid.NewGuid());

        public static readonly IList<TodoItem> PersistedItems = new List<TodoItem>
            {
                new TodoItem { Title = "Should be found (1)", Description = "Some kind of description 1.", ColorName = "Röd", ColorValue = "Red", Created = new SqlDateTime(DateTime.Now).Value, NextOccurrence = new SqlDateTime(DateTime.Now.AddDays(1)).Value, Recurring = 10, StorageKey = TestCollectionKey.Identifier },
                new TodoItem { Title = "Should be found (2)", Description = "Some kind of description 2.", ColorName = "Orange", ColorValue = "Orange", Created = new SqlDateTime(DateTime.Now).Value, NextOccurrence = new SqlDateTime(DateTime.Now.AddDays(7)).Value, Recurring = 5, StorageKey = TestCollectionKey.Identifier },
                new TodoItem { Title = "Should be found (3)", Description = "Some kind of description 3.", ColorName = "Orange", ColorValue = "Orange", Created = new SqlDateTime(DateTime.Now).Value, NextOccurrence = null, Recurring = 0, StorageKey = TestCollectionKey.Identifier },
                new TodoItem { Title = "Should not be found (1)", Description = "Some kind of description 4.", ColorName = "Grön", ColorValue = "Green", Created = new SqlDateTime(DateTime.Now).Value, NextOccurrence = new SqlDateTime(DateTime.Now.AddDays(5)).Value, Recurring = 0, StorageKey = Guid.NewGuid() },
                new TodoItem { Title = "Should not be found (2)", Description = "Some kind of description 5.", ColorName = "Röd", ColorValue = "Red", Created = new SqlDateTime(DateTime.Now).Value, NextOccurrence = new SqlDateTime(DateTime.Now.AddDays(1)).Value, Recurring = 1, StorageKey = Guid.NewGuid() }
            };

        public static IList<Todo> DomainObjects => 
            PersistedItems.Select(item =>
                new Todo
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Color = new Color(item.ColorName, item.ColorValue),
                    Created = item.Created,
                    Recurring = item.Recurring,
                    NextOccurrence = item.NextOccurrence
                })
            .ToList();
    }
}
