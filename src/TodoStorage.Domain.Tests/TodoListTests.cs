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
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class TodoListTests
    {
        private CollectionKey key;

        private IEnumerable<Todo> todos;

        private TodoList sut;

        private TodoList sameProperties;

        private TodoList[] someDiffering;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            key = fixture.Create<CollectionKey>();
            todos = fixture.CreateMany<Todo>();

            sut = new TodoList(key, todos);

            sameProperties = new TodoList(key, todos);
            someDiffering = new[]
            {
                new TodoList(fixture.Create<CollectionKey>(), todos),
                new TodoList(key, fixture.CreateMany<Todo>())
            };
        }

        [Test]
        public void Ctor_GivenListKey_ConstructsListWithKey()
        {
            Assert.That(sut.Key, Is.EqualTo(key));
        }

        [Test]
        public void Ctor_GivenTodoItems_ConstructsNonEmptyList()
        {
            Assert.That(sut.Items, Is.EquivalentTo(todos));
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            foreach (var otherTodoList in someDiffering)
            {
                Assert.That(sut.Equals(otherTodoList), Is.False);
            }
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                var seed = (start * multiplier) + sut.Key.GetHashCode();
                hash = sut.Items.Aggregate(
                    seed,
                    (h, todo) => unchecked((h * multiplier) + todo.GetHashCode()));
            }

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
