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

namespace TodoStorage.Domain.Tests
{
    using System;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public class TodoListTests
    {
        [Test]
        public void Ctor_GivenListKey_ConstructsListWithKey()
        {
            var key = new CollectionKey(Guid.NewGuid());
            var sut = new TodoList(key);

            Assert.That(sut.Key, Is.EqualTo(key));
        }

        [Test]
        public void Ctor_ConstructsEmptyList()
        {
            var key = new CollectionKey(Guid.NewGuid());
            var sut = new TodoList(key);

            Assert.That(sut.Items, Is.Empty);
        }
    }
}
