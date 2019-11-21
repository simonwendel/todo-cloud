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

namespace TodoStorage.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class TodoServiceTests
    {
        private CollectionKey collectionKey;
        private CollectionKey notOwnersKey;
        private Mock<IAccessControlService> accessControlService;
        private Mock<ITodoRepository> todoRepository;
        private IList<Todo> listOfTodos;
        private Todo newTodo;
        private Todo persistedTodo;
        private TodoService sut;

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
                .Setup(a => a.IsOwnerOf(notOwnersKey, It.IsAny<Todo>()))
                .Returns(false);

            accessControlService
                .Setup(a => a.IsOwnerOf(collectionKey, It.IsAny<Todo>()))
                .Returns(true);

            todoRepository = new Mock<ITodoRepository>();
            todoRepository
                .Setup(r => r.GetAll(It.IsAny<CollectionKey>()))
                .Returns(listOfTodos);

            todoRepository
                .Setup(r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()))
                .Returns(-10);

            sut = new TodoService(accessControlService.Object, todoRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullAccessControlRepository_ThrowsException()
        {
            Action constructing = () => new TodoService(null, todoRepository.Object);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Ctor_GivenNullTodoRepository_ThrowsException()
        {
            Action constructing = () => new TodoService(accessControlService.Object, null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetAll_GivenNullCollectionKey_ThrowsException()
        {
            Action gettingAll = () => sut.GetAll(null);
            gettingAll.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.GetAll(It.IsAny<CollectionKey>()), Times.Never);
        }

        [Test]
        public void GetAll_GivenCollectionKey_ReturnsFromRepo()
        {
            sut.GetAll(collectionKey).Should().BeSameAs(listOfTodos);
            todoRepository.Verify(r => r.GetAll(collectionKey), Times.Once);
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            Action adding = () => sut.Add(null, collectionKey);
            adding.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()), Times.Never);
        }

        [Test]
        public void Add_GivenNullCollectionKey_ThrowsException()
        {
            Action adding = () => sut.Add(newTodo, null);
            adding.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Add(It.IsAny<Todo>(), It.IsAny<CollectionKey>()), Times.Never);
        }

        [Test]
        public void Add_GivenTodoAndCollectionKey_PersistsToRepo()
        {
            sut.Add(newTodo, collectionKey).Should().BeSameAs(newTodo);
            todoRepository.Verify(r => r.Add(newTodo, collectionKey), Times.Once);
        }

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            Action updating = () => sut.Update(null, collectionKey);
            updating.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Update(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Update_GivenNullCollectionKey_ThrowsException()
        {
            Action updating = () => sut.Update(persistedTodo, null);
            updating.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Update(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Update_GivenNotAuthorisedKey_ThrowsException()
        {
            Action updating = () => sut.Update(persistedTodo, notOwnersKey);

            updating.Should().ThrowExactly<AccessControlException>();

            accessControlService.Verify(a => a.IsOwnerOf(notOwnersKey, It.IsAny<Todo>()), Times.Once);
            todoRepository.Verify(r => r.Update(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Update_WhenGettingFailureFromRepo_ThrowsException()
        {
            todoRepository.Setup(r => r.Update(persistedTodo)).Returns(false);

            Action updating = () => sut.Update(persistedTodo, collectionKey);
            updating.Should().ThrowExactly<UpdateFailedException>();

            accessControlService.Verify(a => a.IsOwnerOf(collectionKey, It.IsAny<Todo>()), Times.Once);
            todoRepository.Verify(r => r.Update(It.Is<Todo>(t => t == persistedTodo)), Times.Once);
        }

        [Test]
        public void Update_WhenGettingSuccessFromRepo_Continues()
        {
            todoRepository.Setup(r => r.Update(persistedTodo)).Returns(true);

            sut.Update(persistedTodo, collectionKey);

            accessControlService.Verify(
                a => a.IsOwnerOf(collectionKey, It.IsAny<Todo>()),
                Times.Once);

            todoRepository.Verify(r => r.Update(persistedTodo), Times.Once);
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            Action deleting = () => sut.Delete(null, collectionKey);
            deleting.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Delete(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Delete_GivenNullCollectionKey_ThrowsException()
        {
            Action deleting = () => sut.Delete(persistedTodo, null);
            deleting.Should().ThrowExactly<ArgumentNullException>();
            todoRepository.Verify(r => r.Delete(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Delete_GivenNotAuthorisedKey_ThrowsException()
        {
            Action deleting = () => sut.Delete(persistedTodo, notOwnersKey);
            deleting.Should().ThrowExactly<AccessControlException>();

            accessControlService.Verify(a => a.IsOwnerOf(notOwnersKey, It.IsAny<Todo>()), Times.Once);
            todoRepository.Verify(r => r.Delete(It.IsAny<Todo>()), Times.Never);
        }

        [Test]
        public void Delete_WhenGettingFailureFromRepo_ThrowsException()
        {
            todoRepository.Setup(r => r.Delete(persistedTodo)).Returns(false);

            Action deleting = () => sut.Delete(persistedTodo, collectionKey);
            deleting.Should().ThrowExactly<DeleteFailedException>();

            accessControlService.Verify(a => a.IsOwnerOf(collectionKey, It.IsAny<Todo>()), Times.Once);
            todoRepository.Verify(r => r.Delete(persistedTodo), Times.Once);
        }

        [Test]
        public void Delete_WhenGettingSuccessFromRepo_Continues()
        {
            todoRepository.Setup(r => r.Delete(persistedTodo)).Returns(true);

            sut.Delete(persistedTodo, collectionKey);

            accessControlService.Verify(a => a.IsOwnerOf(collectionKey, It.IsAny<Todo>()), Times.Once);
            todoRepository.Verify(r => r.Delete(It.Is<Todo>(t => t == persistedTodo)), Times.Once);
        }
    }
}
