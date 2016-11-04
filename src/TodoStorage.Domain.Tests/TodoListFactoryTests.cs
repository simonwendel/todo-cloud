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
    using Domain;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class TodoListFactoryTests
    {
        private TodoListFactory sut;

        private CollectionKey key;

        private IEnumerable<Todo> someTodo;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            key = fixture.Create<CollectionKey>();
            someTodo = fixture.CreateMany<Todo>();

            sut = new TodoListFactory();
        }

        [Test]
        public void Create_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.Create(null, someTodo);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Create_GivenNullTodo_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.Create(key, null);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Create_GivenKeyAndTodo_ReturnsTodoList()
        {
            var result = sut.Create(key, someTodo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Key, Is.SameAs(key));
            Assert.That(result.Items, Is.EquivalentTo(someTodo));
        }
    }
}
