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

namespace TodoStorage.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Results;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Ploeh.Hyprlinkr;
    using TodoStorage.Api.Controllers;
    using TodoStorage.Domain;

    [TestFixture]
    internal class TodoControllerTests : ControllerTestFixtureBase, IDisposable
    {
        private IReadOnlyList<Todo> items;

        private Mock<ITodoList> todoList;

        private Mock<ITodoListFactory> todoListFactory;

        private Func<HttpRequestMessage, IResourceLinker> linkerStrategy;

        private Mock<IResourceLinker> resourceLinker;

        private Uri redirectLocation;

        private Todo newTodo;

        private Todo existingTodo;

        private TodoController sut;

        [SetUp]
        public void Setup()
        {
            SetPrincipal(Guid.NewGuid());

            CreateTodoListMock();
            CreateTodoListFactoryMock();
            CreateResourceLinkerMock();

            linkerStrategy = 
                request => resourceLinker.Object;

            sut = new TodoController(todoListFactory.Object, linkerStrategy);
        }

        [Test]
        public void Ctor_GivenNullTodoListFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoController(null, linkerStrategy);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullLinkerStrategy_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoController(todoListFactory.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenTodoListFactory_RetrievesTodoList()
        {
            todoListFactory.Verify(
                f => f.Create(It.IsAny<CollectionKey>()),
                Times.Once);
        }

        [Test]
        public void Get_NullaryInvocation_ReturnsTodoItems()
        {
            var todo = sut.Get();

            Assert.That(todo, Is.EquivalentTo(items));
        }

        [Test]
        public void Post_GivenNullTodo_ThrowsException()
        {
            TestDelegate postCall =
                () => sut.Post(null);

            Assert.That(postCall, Throws.ArgumentNullException);
            todoList.Verify(
                l => l.Add(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Post_GivenTodo_AddsTodoToList()
        {
            sut.Post(newTodo);

            todoList.Verify(
                l => l.Add(It.Is<Todo>(t => t == newTodo)),
                Times.Once);
        }

        [Test]
        public void Post_GivenTodo_ReturnsCreatedResponse()
        {
            var response = sut.Post(newTodo) as CreatedNegotiatedContentResult<Todo>;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(
                response.Location, 
                Is.EqualTo(redirectLocation));
        }

        [Test]
        public void Put_GivenNullTodo_ThrowsException()
        {
            TestDelegate putCall =
                () => sut.Put(null);

            Assert.That(putCall, Throws.ArgumentNullException);

            todoList.Verify(
                 l => l.Add(It.IsAny<Todo>()),
                 Times.Never);
            todoList.Verify(
                l => l.Update(It.IsAny<Todo>()),
                Times.Never);
        }

        [Test]
        public void Put_GivenExistingTodo_UpdatesTodo()
        {
            sut.Put(existingTodo);

            todoList.Verify(
                l => l.Update(It.Is<Todo>(t => t == existingTodo)),
                Times.Once);
        }

        [Test]
        public void Put_GivenExistingTodo_ReturnsOkResponse()
        {
            var response = sut.Put(existingTodo) as OkNegotiatedContentResult<Todo>;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Null);
        }

        [Test]
        public void Put_GivenNewTodo_AddsTodo()
        {
            sut.Put(newTodo);

            todoList.Verify(
                l => l.Add(It.Is<Todo>(t => t == newTodo)),
                Times.Once);
        }

        [Test]
        public void Put_GivenNewTodo_ReturnsCreatedResponse()
        {
            var response = sut.Put(newTodo) as CreatedNegotiatedContentResult<Todo>;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(
                response.Location,
                Is.EqualTo(redirectLocation));
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            TestDelegate deleteCall =
                () => sut.Delete(null);

            Assert.That(deleteCall, Throws.ArgumentNullException);
            todoList.Verify(
                 l => l.Delete(It.IsAny<Todo>()),
                 Times.Never);
        }

        [Test]
        public void Delete_GivenUnrecognizedTodo_ReturnsNotFoundResponse()
        {
            var response = sut.Delete(newTodo) as NotFoundResult;

            Assert.That(response, Is.Not.Null);
            todoList.Verify(
                l => l.Delete(It.Is<Todo>(t => t == newTodo)),
                Times.Never);
        }

        [Test]
        public void Delete_GivenExistingTodo_DeletesTodo()
        {
            sut.Delete(existingTodo);

            todoList.Verify(
                l => l.Delete(It.Is<Todo>(t => t == existingTodo)),
                Times.Once);
        }

        [Test]
        public void Delete_GivenExistingTodo_ReturnsNoContentResponse()
        {
            var response = sut.Delete(existingTodo) as StatusCodeResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sut == null)
                {
                    sut.Dispose();
                    sut = null;
                }
            }
        }

        #endregion

        private void CreateTodoListMock()
        {
            var fixture = new Fixture();

            items = fixture
                .CreateMany<Todo>()
                .ToList()
                .AsReadOnly();

            newTodo = fixture.Create<Todo>();
            existingTodo = items[0];

            todoList = new Mock<ITodoList>();
            todoList
                .SetupGet(l => l.Items)
                .Returns(items);

            todoList
                .Setup(l => l.Add(It.IsAny<Todo>()));

            todoList
                .Setup(l => l.Update(It.IsAny<Todo>()));
        }

        private void CreateTodoListFactoryMock()
        {
            todoListFactory = new Mock<ITodoListFactory>();
            todoListFactory
                .Setup(f => f.Create(It.IsAny<CollectionKey>()))
                .Returns(todoList.Object);
        }

        private void CreateResourceLinkerMock()
        {
            redirectLocation = new Uri("http://localhost/api/todo/whatever");

            resourceLinker = new Mock<IResourceLinker>();
            resourceLinker
                .Setup(r => r.GetUri<TodoController>(It.IsAny<Expression<Action<TodoController>>>()))
                .Returns(redirectLocation);
        }
    }
}
