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

namespace TodoStorage.Persistence.Tests.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ploeh.AutoFixture;
    using TodoStorage.Core;
    using TodoStorage.Persistence.Tests.Utilities;

    internal static class Data
    {
        public static readonly CollectionKey TestCollectionKey = new CollectionKey(Guid.NewGuid());

        public static readonly CollectionKey NonPersistedCollectionKey = new CollectionKey(Guid.NewGuid());

        public static readonly AuthenticationItem Auth = new AuthenticationItem
        {
            AppId = TestCollectionKey.Identifier,
            AccountName = "Account number 1",
            Secret = ConstructSecret()
        };

        public static readonly AuthenticationItem OtherAuth = new AuthenticationItem
        {
            AppId = Guid.NewGuid(),
            AccountName = "Account number 2",
            Secret = ConstructSecret()
        };

        public static readonly AuthenticationItem EmptyAuth = new AuthenticationItem
        {
            AppId = NonPersistedCollectionKey.Identifier,
            AccountName = "Account number 3",
            Secret = ConstructSecret()
        };

        public static readonly IList<TodoItem> PersistedItems = new List<TodoItem>
            {
                new TodoItem { Title = "Should be found (1)", Description = "Some kind of description 1.", ColorValue = Color.SeaGreen.Value, Created = DateTime.Now.SqlNormalize(), NextOccurrence = DateTime.Now.AddDays(1).SqlNormalize(), Recurring = 10, AppId = Auth.AppId },
                new TodoItem { Title = "Should be found (2)", Description = "Some kind of description 2.", ColorValue = Color.Violet.Value, Created = DateTime.Now.SqlNormalize(), NextOccurrence = DateTime.Now.AddDays(7).SqlNormalize(), Recurring = 5, AppId = Auth.AppId },
                new TodoItem { Title = "Should be found (3)", Description = "Some kind of description 3.", ColorValue = Color.Violet.Value, Created = DateTime.Now.SqlNormalize(), NextOccurrence = null, Recurring = 0, AppId = Auth.AppId },
                new TodoItem { Title = "Should not be found (1)", Description = "Some kind of description 4.", ColorValue = Color.SeaGreen.Value, Created = DateTime.Now.SqlNormalize(), NextOccurrence = DateTime.Now.AddDays(5).SqlNormalize(), Recurring = 0, AppId = OtherAuth.AppId },
                new TodoItem { Title = "Should not be found (2)", Description = "Some kind of description 5.", ColorValue = Color.Crimson.Value, Created = DateTime.Now.SqlNormalize(), NextOccurrence = DateTime.Now.AddDays(1).SqlNormalize(), Recurring = 1, AppId = OtherAuth.AppId }
            };

        public static IList<Todo> OwnedByTestKey =>
            GetTodoItems(item => item.AppId.Equals(TestCollectionKey.Identifier));

        public static IList<Todo> NotOwnedByTestKey =>
            GetTodoItems(item => !item.AppId.Equals(TestCollectionKey.Identifier));

        private static IList<Todo> GetTodoItems(Func<TodoItem, bool> predicate)
        {
            return PersistedItems
                .Where(predicate)
                .OrderBy(item => item.Id)
                .Select(item =>
                    new Todo
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Color = Color.Pick(item.ColorValue),
                        Created = item.Created,
                        Recurring = item.Recurring,
                        NextOccurrence = item.NextOccurrence
                    })
                .ToList();
        }

        private static byte[] ConstructSecret()
        {
            var fixture = new Fixture();
            return fixture.CreateMany<byte>(32).ToArray();
        }
    }
}
