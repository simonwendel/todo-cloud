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

        private CollectionKey notOwnersKey;

        private Mock<IAccessControlService> accessControlService;

        private Mock<ITodoRepository> todoRepository;

        private IList<Todo> listOfTodos;

        private Todo newTodo;

        private Todo persistedTodo;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            collectionKey = fixture.Create<CollectionKey>();
            notOwnersKey = fixture.Create<CollectionKey>();
            listOfTodos = fixture.CreateMany<Todo>().ToList();

            newTodo = fixture.Create<Todo>();
            persistedTodo = fixture.Create<Todo>();

            accessControlService = new Mock<IAccessControlService>();
            accessControlService
                .Setup(a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == notOwnersKey), It.IsAny<Todo>()))
                .Returns(false);

            accessControlService
                .Setup(a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == collectionKey), It.IsAny<Todo>()))
                .Returns(true);

            todoRepository = new Mock<ITodoRepository>();
            todoRepository
                .Setup(r => r.GetAll(It.IsAny<CollectionKey>()))
                .Returns(listOfTodos);

            todoRepository
                .Setup(r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()))
                .Returns(persistedTodo);

            sut = new TodoService(accessControlService.Object, todoRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullAccessControlRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoService(null, todoRepository.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullTodoRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoService(accessControlService.Object, null);

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

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            TestDelegate updateCall =
                () => sut.Update(null, collectionKey);

            Assert.That(updateCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Update(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Update_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate updateCall =
                () => sut.Update(persistedTodo, null);

            Assert.That(updateCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Update(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Update_GivenNotAuthorisedKey_ThrowsException()
        {
            TestDelegate updateCall =
                () => sut.Update(persistedTodo, notOwnersKey);

            Assert.That(updateCall, Throws.TypeOf<AccessControlException>());

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == notOwnersKey), It.IsAny<Todo>()), 
                Times.Once);

            todoRepository.Verify(
                r => r.Update(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Update_WhenGettingFailureFromRepo_ThrowsException()
        {
            todoRepository
                .Setup(r => r.Update(It.Is<Todo>(t => t == persistedTodo)))
                .Returns(false);

            TestDelegate updateCall =
                () => sut.Update(persistedTodo, collectionKey);

            Assert.That(updateCall, Throws.TypeOf<UpdateFailedException>());

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == collectionKey), It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(
                r => r.Update(It.Is<Todo>(t => t == persistedTodo)),
                Times.Once);
        }

        [Test]
        public void Update_WhenGettingSuccessFromRepo_Continues()
        {
            todoRepository
                .Setup(r => r.Update(It.Is<Todo>(t => t == persistedTodo)))
                .Returns(true);

            sut.Update(persistedTodo, collectionKey);

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == collectionKey), It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(
                r => r.Update(It.Is<Todo>(t => t == persistedTodo)),
                Times.Once);
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            TestDelegate deleteCall =
                () => sut.Delete(null, collectionKey);

            Assert.That(deleteCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Delete(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Delete_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate deleteCall =
                () => sut.Delete(persistedTodo, null);

            Assert.That(deleteCall, Throws.ArgumentNullException);
            todoRepository.Verify(
                r => r.Delete(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Delete_GivenNotAuthorisedKey_ThrowsException()
        {
            TestDelegate deleteCall =
                () => sut.Delete(persistedTodo, notOwnersKey);

            Assert.That(deleteCall, Throws.TypeOf<AccessControlException>());

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == notOwnersKey), It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(
                r => r.Delete(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Delete_WhenGettingFailureFromRepo_ThrowsException()
        {
            todoRepository
                .Setup(r => r.Delete(It.Is<Todo>(t => t == persistedTodo)))
                .Returns(false);

            TestDelegate deleteCall =
                () => sut.Delete(persistedTodo, collectionKey);

            Assert.That(deleteCall, Throws.TypeOf<DeleteFailedException>());

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == collectionKey), It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(
                r => r.Delete(It.Is<Todo>(t => t == persistedTodo)),
                Times.Once);
        }

        [Test]
        public void Delete_WhenGettingSuccessFromRepo_Continues()
        {
            todoRepository
                .Setup(r => r.Delete(It.Is<Todo>(t => t == persistedTodo)))
                .Returns(true);

            sut.Delete(persistedTodo, collectionKey);

            accessControlService.Verify(
                a => a.IsOwnerOf(It.Is<CollectionKey>(k => k == collectionKey), It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(
                r => r.Delete(It.Is<Todo>(t => t == persistedTodo)),
                Times.Once);
        }
    }
}
