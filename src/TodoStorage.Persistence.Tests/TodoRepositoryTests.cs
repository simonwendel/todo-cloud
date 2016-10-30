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
    using Domain.Data;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Seed;
    using Utilities;

    [TestFixture]
    internal class TodoRepositoryTests
    {
        private TodoRepository sut;

        private CollectionKey collectionKey;

        private Todo newTodo;

        [SetUp]
        public void Setup()
        {
            var seeder = new DatabaseSeeder();
            seeder.InjectSeed();

            var resolver = new ConnectionStringResolver("TodoStorage");
            var connectionFactory = new SqlServerConnectionFactory(resolver);

            var fixture = new Fixture();
            newTodo = fixture.Create<Todo>();
            newTodo.Color = new Color("cname", "cvalue");

            newTodo.Created = newTodo.Created.SqlNormalize();
            newTodo.NextOccurrence = newTodo.NextOccurrence.SqlNormalize();

            collectionKey = fixture.Create<CollectionKey>();

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
            var actual = sut.GetTodo(collectionKey);

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

        [Test]
        public void Delete_GivenId_DeletesTodoAndReturnsTrue()
        {
            var first = Seed.Data.OwnedByTestKey.First();
            Seed.Data.OwnedByTestKey.Remove(first);

            var id = first.Id;

            var expectedLeft = Seed.Data.OwnedByTestKey.Count - 1;
            var key = Seed.Data.TestCollectionKey;

            var succeeded = sut.Delete(id.Value);
            var actualLeft = sut.GetTodo(key).Count;

            Assert.That(succeeded, Is.True);
            Assert.That(actualLeft, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Delete_GivenNonExistentId_DoesntDeleteAndReturnsFalse()
        {
            var id = Seed.Data.OwnedByTestKey.Sum(t => t.Id.Value) + 1;
            var expectedLeft = Seed.Data.OwnedByTestKey.Count;
            var key = Seed.Data.TestCollectionKey;

            var succeeded = sut.Delete(id);
            var actualLeft = sut.GetTodo(key).Count;

            Assert.That(succeeded, Is.False);
            Assert.That(actualLeft, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(null, collectionKey);

            Assert.That(addCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Add_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(newTodo, null);

            Assert.That(addCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_PersistsTodo()
        {
            var prior = sut.GetTodo(collectionKey);
            var todo = sut.Add(newTodo, collectionKey);
            var persisted = sut.GetTodo(collectionKey);

            Assert.That(prior, Is.Empty);
            Assert.That(persisted, Is.EquivalentTo(new[] { todo }));
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_UpdatesIdOfTodo()
        {
            newTodo.Id = -1;
            var oldId = newTodo.Id;

            var persisted = sut.Add(newTodo, collectionKey);

            Assert.That(persisted.Id, Is.Not.EqualTo(oldId));
            Assert.That(persisted, Is.EqualTo(newTodo));
        }
    }
}
