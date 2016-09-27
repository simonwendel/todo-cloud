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

namespace TodoStorage.Persistence.Tests
{
    using Domain.Data;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class TodoRepositoryTests
    {
        private TodoRepository sut;

        [SetUp]
        public void Setup()
        {
            var resolver = new ConnectionStringResolver("TodoStorage");
            var connectionFactory = new SqlServerConnectionFactory(resolver);

            sut = new TodoRepository(connectionFactory);
        }

        [Test]
        public void Ctor_GivenNullDbConnectionFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoRepository(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetTodo_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate getTodoCall =
                 () => sut.GetTodo(null);

            Assert.That(getTodoCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetTodo_GivenNonPersistedCollectionKey_ReturnsEmptyList()
        {
            var fixture = new Fixture();
            var key = fixture.Create<CollectionKey>();

            var actual = sut.GetTodo(key);

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetTodo_GivenCollectionKey_ReturnsTodo()
        {
            var key = Seed.Data.TestCollectionKey;
            var expected = Seed.Data.OwnedByTestKey;

            var actual = sut.GetTodo(key);

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
