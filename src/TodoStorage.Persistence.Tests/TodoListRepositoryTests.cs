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
    internal class TodoListRepositoryTests
    {
        private SqlServerConnectionFactory connectionFactory;

        private TodoListRepository sut;

        [SetUp]
        public void Setup()
        {
            var resolver = new ConnectionStringResolver("TodoStorage");

            connectionFactory = new SqlServerConnectionFactory(resolver);
            sut = new TodoListRepository(connectionFactory);
        }

        [Test]
        public void Ctor_GivenNullDbConnectionFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoListRepository(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetList_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate getListCall =
                 () => sut.GetList(null);

            Assert.That(getListCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetList_GivenNonPersistedCollectionKey_ReturnsNull()
        {
            var fixture = new Fixture();
            var key = fixture.Create<CollectionKey>();

            var actual = sut.GetList(key);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetList_GivenCollectionKey_ReturnsTodoList()
        {
            var key = Seed.Data.TestCollectionKey;
            var expected = Seed.Data.TestList;

            var actual = sut.GetList(key);

            Assert.That(actual.Equals(expected));
        }
    }
}
