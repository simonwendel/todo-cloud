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
    using TodoStorage.Persistence.Tests.Utilities;

    internal class DatabaseSeeder
    {
        public void InjectSeed()
        {
            using (var context = new TodoStorageSeedContext())
            {
                context.AuthenticationItems.Clear();
                context.AuthenticationItems.Add(Data.Auth);
                context.AuthenticationItems.Add(Data.OtherAuth);
                context.AuthenticationItems.Add(Data.EmptyAuth);

                context.TodoItems.Clear();
                foreach (var item in Data.PersistedItems)
                {
                    context.TodoItems.Add(item);
                }

                context.SaveChanges();
            }
        }
    }
}
