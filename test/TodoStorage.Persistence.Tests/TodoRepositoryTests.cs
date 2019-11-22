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

namespace TodoStorage.Persistence.Tests
{
    using System;
    using System.Linq;
    using AutoFixture;
    using FluentAssertions;
    using NUnit.Framework;
    using SimonWendel.ObjectExtensions;
    using TodoStorage.Core;
    using TodoStorage.Persistence.Tests.Seed;
    using TodoStorage.Persistence.Tests.Utilities;

    [TestFixture]
    internal class TodoRepositoryTests
    {
        private CollectionKey nonPersistedCollectionKey;
        private Todo newTodo;
        private Todo persistedTodo;
        private TodoRepository sut;

        [SetUp]
        public void Setup()
        {
            var seeder = new DatabaseSeeder();
            seeder.InjectSeed();

            var resolver = new ConnectionStringResolver("TodoStorageTest");
            var connectionFactory = new ConnectionFactory(resolver);
            sut = new TodoRepository(connectionFactory);

            var fixture = new Fixture();

            nonPersistedCollectionKey = Seed.Data.NonPersistedCollectionKey;

            persistedTodo = Seed.Data.OwnedByTestKey.First();
            var nonPersistedId = Seed.Data.OwnedByTestKey.Sum(t => t.Id.Value) + 1;

            newTodo = fixture.Create<Todo>();
            newTodo
                .SetProperty(t => t.Id, nonPersistedId)
                .SetProperty(t => t.Color, Color.DarkBlue)
                .SetProperty(t => t.Created, newTodo.Created.SqlNormalize())
                .SetProperty(t => t.NextOccurrence, newTodo.NextOccurrence.SqlNormalize());
        }

        [Test]
        public void Ctor_GivenNullDbConnectionFactory_ThrowsException()
        {
            Action constructing = () => new TodoRepository(null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetAll_GivenNullCollectionKey_ThrowsException()
        {
            Action gettingAll = () => sut.GetAll(null);
            gettingAll.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetAll_GivenNonPersistedCollectionKey_ReturnsEmptyList()
        {
            sut.GetAll(nonPersistedCollectionKey).Should().BeEmpty();
        }

        [Test]
        public void GetAll_GivenCollectionKey_ReturnsTodo()
        {
            var key = Seed.Data.TestCollectionKey;
            var expected = Seed.Data.OwnedByTestKey;
            sut.GetAll(key).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            Action deleting = () => sut.Delete(null);
            deleting.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Delete_GivenTodo_DeletesTodoAndReturnsTrue()
        {
            Seed.Data.OwnedByTestKey.Remove(persistedTodo);

            var expectedLeft = Seed.Data.OwnedByTestKey.Count - 1;
            var key = Seed.Data.TestCollectionKey;

            sut.Delete(persistedTodo).Should().BeTrue();
            sut.GetAll(key).Should().HaveCount(expectedLeft);
        }

        [Test]
        public void Delete_GivenTodoWithNoId_DoesntDeleteAndReturnsFalse()
        {
            var todoWithNoId = new Todo();

            var expectedLeft = Seed.Data.OwnedByTestKey.Count;
            var key = Seed.Data.TestCollectionKey;

            sut.Delete(todoWithNoId).Should().BeFalse();
            sut.GetAll(key).Should().HaveCount(expectedLeft);
        }

        [Test]
        public void Delete_GivenNonExistentTodo_DoesntDeleteAndReturnsFalse()
        {
            var expectedLeft = Seed.Data.OwnedByTestKey.Count;
            var key = Seed.Data.TestCollectionKey;

            sut.Delete(newTodo).Should().BeFalse();
            sut.GetAll(key).Should().HaveCount(expectedLeft);
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            Action adding = () => sut.Add(null, nonPersistedCollectionKey);
            adding.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Add_GivenNullCollectionKey_ThrowsException()
        {
            Action adding = () => sut.Add(newTodo, null);
            adding.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_PersistsTodo()
        {
            sut.GetAll(nonPersistedCollectionKey).Should().BeEmpty();
            
            var persistedId = sut.Add(newTodo, nonPersistedCollectionKey);
            var persisted = sut.GetAll(nonPersistedCollectionKey);

            var todo = newTodo.SetProperty(t => t.Id, persistedId);
            persisted.Should().BeEquivalentTo(todo);
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_UpdatesIdOfTodo()
        {
            var oldId = newTodo.Id;
            sut.Add(newTodo, nonPersistedCollectionKey).Should().NotBe(oldId);
        }

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            Action updating = () => sut.Update(null);
            updating.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Update_GivenNonPersistedTodo_DoesntPersistAndReturnsFalse()
        {
            sut.Update(newTodo).Should().BeFalse();
            sut.GetAll(Seed.Data.TestCollectionKey).Should().NotBeEmpty().And.NotContain(newTodo);
        }

        [Test]
        public void Update_GivenPersistedTodo_PersistsChangesAndReturnsTrue()
        {
            // use persisted id with totally new data
            var changedTodo = newTodo.SetProperty(t => t.Id, persistedTodo.Id);
            sut.Update(changedTodo).Should().BeTrue();
            sut.GetAll(Seed.Data.TestCollectionKey).Should().NotBeEmpty().And.Contain(changedTodo);
        }
    }
}
