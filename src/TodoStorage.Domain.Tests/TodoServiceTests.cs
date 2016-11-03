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
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class TodoServiceTests
    {
        private TodoService sut;

        private CollectionKey collectionKey;

        private Mock<ITodoRepository> todoRepository;

        private IList<Todo> listOfTodos;

        private Todo newTodo;

        private Todo persistedTodo;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            collectionKey = fixture.Create<CollectionKey>();
            listOfTodos = fixture.CreateMany<Todo>().ToList();

            newTodo = fixture.Create<Todo>();
            persistedTodo = fixture.Create<Todo>();

            todoRepository = new Mock<ITodoRepository>();
            todoRepository
                .Setup(r => r.GetAll(It.IsAny<CollectionKey>()))
                .Returns(listOfTodos);

            todoRepository
                .Setup(r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()))
                .Returns(persistedTodo);

            sut = new TodoService(todoRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullTodoRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoService(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetAll_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate getAllCall =
                () => sut.GetAll(null);

            Assert.That(getAllCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.GetAll(It.IsAny<CollectionKey>()),
                Times.Never);
        }

        [Test]
        public void GetAll_GivenCollectionKey_ReturnsFromRepo()
        {
            var actualTodos = sut.GetAll(collectionKey);

            Assert.That(actualTodos, Is.SameAs(listOfTodos));
            todoRepository.Verify(
                r => r.GetAll(It.Is<CollectionKey>(c => c.Equals(collectionKey))), 
                Times.Once);
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(null, collectionKey);

            Assert.That(addCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()),
                Times.Never);
        }

        [Test]
        public void Add_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(newTodo, null);

            Assert.That(addCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()),
                Times.Never);
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_PersistsToRepo()
        {
            var actualTodo = sut.Add(newTodo, collectionKey);

            Assert.That(actualTodo, Is.SameAs(persistedTodo));
            todoRepository.Verify(
                r => r.Add(It.Is<Todo>(t => t == newTodo), It.Is<CollectionKey>(k => k == collectionKey)),
                Times.Once);
        }
    }
}
