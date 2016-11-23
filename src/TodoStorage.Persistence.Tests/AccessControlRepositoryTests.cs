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
    using System.Linq;
    using NUnit.Framework;
    using TodoStorage.Persistence.Tests.Seed;

    [TestFixture]
    internal class AccessControlRepositoryTests
    {
        private AccessControlRepository sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var seeder = new DatabaseSeeder();
            seeder.InjectSeed();
        }

        [SetUp]
        public void Setup()
        {
            var resolver = new ConnectionStringResolver("TodoStorageTest");
            var connectionFactory = new ConnectionFactory(resolver);

            sut = new AccessControlRepository(connectionFactory);
        }

        [Test]
        public void Ctor_GivenNullDbConnectionFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AccessControlRepository(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                 () => sut.IsOwnerOf(null, 0);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_GivenOwningCollectionKey_ReturnsTrue()
        {
            var ownerKey = Seed.Data.TestCollectionKey;
            var ownedTodoId = Seed.Data.OwnedByTestKey.First().Id.Value;

            var ownsThatTodo = sut.IsOwnerOf(ownerKey, ownedTodoId);

            Assert.That(ownsThatTodo, Is.True);
        }

        [Test]
        public void IsOwnerOf_GivenNonOwningCollectionKey_ReturnsFalse()
        {
            var ownerKey = Seed.Data.TestCollectionKey;
            var ownedTodoId = Seed.Data.NotOwnedByTestKey.First().Id.Value;

            var ownsThatTodo = sut.IsOwnerOf(ownerKey, ownedTodoId);

            Assert.That(ownsThatTodo, Is.False);
        }
    }
}
