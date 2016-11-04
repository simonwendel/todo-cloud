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
    using Domain;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Seed;
    using SimonWendel.ObjectExtensions;
    using Utilities;

    [TestFixture]
    internal class TodoRepositoryTests
    {
        private TodoRepository sut;

        private CollectionKey nonPersistedCollectionKey;

        private Todo newTodo;

        private Todo persistedTodo;

        [SetUp]
        public void Setup()
        {
            var seeder = new DatabaseSeeder();
            seeder.InjectSeed();

            var resolver = new ConnectionStringResolver("TodoStorage");
            var connectionFactory = new SqlServerConnectionFactory(resolver);
            sut = new TodoRepository(connectionFactory);

            var fixture = new Fixture();

            nonPersistedCollectionKey = fixture.Create<CollectionKey>();

            persistedTodo = Seed.Data.OwnedByTestKey.First();
            var nonPersistedId = Seed.Data.OwnedByTestKey.Sum(t => t.Id.Value) + 1;

            newTodo = fixture.Create<Todo>();
            newTodo
                .SetProperty(t => t.Id, nonPersistedId)
                .SetProperty(t => t.Color, new Color("cname", "cvalue"))
                .SetProperty(t => t.Created, newTodo.Created.SqlNormalize())
                .SetProperty(t => t.NextOccurrence, newTodo.NextOccurrence.SqlNormalize());
        }

        [Test]
        public void Ctor_GivenNullDbConnectionFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoRepository(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetAll_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate getAllCall =
                 () => sut.GetAll(null);

            Assert.That(getAllCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetAll_GivenNonPersistedCollectionKey_ReturnsEmptyList()
        {
            var actual = sut.GetAll(nonPersistedCollectionKey);

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetAll_GivenCollectionKey_ReturnsTodo()
        {
            var key = Seed.Data.TestCollectionKey;
            var expected = Seed.Data.OwnedByTestKey;

            var actual = sut.GetAll(key);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            TestDelegate deleteCall =
                 () => sut.Delete(null);

            Assert.That(deleteCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Delete_GivenTodo_DeletesTodoAndReturnsTrue()
        {
            Seed.Data.OwnedByTestKey.Remove(persistedTodo);

            var expectedLeft = Seed.Data.OwnedByTestKey.Count - 1;
            var key = Seed.Data.TestCollectionKey;

            var succeeded = sut.Delete(persistedTodo);
            var actualLeft = sut.GetAll(key).Count;

            Assert.That(succeeded, Is.True);
            Assert.That(actualLeft, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Delete_GivenTodoWithNoId_DoesntDeleteAndReturnsFalse()
        {
            newTodo.Id = null;

            var expectedLeft = Seed.Data.OwnedByTestKey.Count;
            var key = Seed.Data.TestCollectionKey;

            var succeeded = sut.Delete(newTodo);
            var actualLeft = sut.GetAll(key).Count;

            Assert.That(succeeded, Is.False);
            Assert.That(actualLeft, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Delete_GivenNonExistentTodo_DoesntDeleteAndReturnsFalse()
        {
            var expectedLeft = Seed.Data.OwnedByTestKey.Count;
            var key = Seed.Data.TestCollectionKey;

            var succeeded = sut.Delete(newTodo);
            var actualLeft = sut.GetAll(key).Count;

            Assert.That(succeeded, Is.False);
            Assert.That(actualLeft, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(null, nonPersistedCollectionKey);

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
            var prior = sut.GetAll(nonPersistedCollectionKey);
            var todo = sut.Add(newTodo, nonPersistedCollectionKey);
            var persisted = sut.GetAll(nonPersistedCollectionKey);

            Assert.That(prior, Is.Empty);
            Assert.That(persisted, Is.EquivalentTo(new[] { todo }));
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_UpdatesIdOfTodo()
        {
            var oldId = newTodo.Id;

            var persisted = sut.Add(newTodo, nonPersistedCollectionKey);

            Assert.That(persisted.Id, Is.Not.EqualTo(oldId));
            Assert.That(persisted, Is.EqualTo(newTodo));
        }

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            TestDelegate updateCall =
                () => sut.Update(null);

            Assert.That(updateCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Update_GivenNonPersistedTodo_DoesntPersistAndReturnsFalse()
        {
            var didUpdate = sut.Update(newTodo);
            var todosNow = sut.GetAll(Seed.Data.TestCollectionKey);

            Assert.That(didUpdate, Is.False);
            Assert.That(todosNow, Is.Not.Empty);
            CollectionAssert.DoesNotContain(todosNow, newTodo);
        }

        [Test]
        public void Update_GivenPersistedTodo_PersistsChangesAndReturnsTrue()
        {
            // use persisted id with totally new data
            var changedTodo = newTodo.SetProperty(t => t.Id, persistedTodo.Id);

            var didUpdate = sut.Update(changedTodo);
            var todosNow = sut.GetAll(Seed.Data.TestCollectionKey);

            Assert.That(didUpdate, Is.True);
            Assert.That(todosNow, Is.Not.Empty);
            CollectionAssert.Contains(todosNow, changedTodo);
        }
    }
}
