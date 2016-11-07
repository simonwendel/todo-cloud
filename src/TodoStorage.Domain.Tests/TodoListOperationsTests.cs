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

    /// <summary>
    /// Unit tests the more domain-centric list operations for the <see cref="TodoList"/> class.
    /// </summary>
    [TestFixture]
    internal class TodoListOperationsTests
    {
        private TodoList sut;

        private Mock<ITodoService> todoService;

        private CollectionKey key;

        private IList<Todo> todos;

        private Todo someTodo;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            key = fixture.Create<CollectionKey>();
            todos = fixture.CreateMany<Todo>().ToList();
            someTodo = fixture.Create<Todo>();

            todoService = new Mock<ITodoService>();
            todoService
                .Setup(s => s.GetAll(It.Is<CollectionKey>(k => k == key)))
                .Returns(todos);

            sut = new TodoList(todoService.Object, key);
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            TestDelegate addCall =
                () => sut.Add(null);

            Assert.That(addCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Add_GivenTodo_AddsViaTodoService()
        {
            sut.Add(someTodo);

            todoService.Verify(
                s => s.Add(It.Is<Todo>(t => t == someTodo), It.Is<CollectionKey>(k => k == key)),
                Times.Once);
        }

        [Test]
        public void Add_GivenTodo_RefreshesFromTodoService()
        {
            todoService.Verify(
                s => s.GetAll(It.IsAny<CollectionKey>()),
                Times.Once);

            sut.Add(someTodo);

            todoService.Verify(
                s => s.GetAll(It.IsAny<CollectionKey>()),
                Times.Exactly(2));
        }

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            TestDelegate updateCall =
                () => sut.Update(null);

            Assert.That(updateCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Update_GivenTodo_UpdatesViaTodoService()
        {
            sut.Update(someTodo);

            todoService.Verify(
                s => s.Update(It.Is<Todo>(t => t == someTodo), It.Is<CollectionKey>(k => k == key)),
                Times.Once);
        }

        [Test]
        public void Update_GivenTodo_RefreshesFromTodoService()
        {
            todoService.Verify(
                s => s.GetAll(It.IsAny<CollectionKey>()),
                Times.Once);

            sut.Update(someTodo);

            todoService.Verify(
                s => s.GetAll(It.IsAny<CollectionKey>()),
                Times.Exactly(2));
        }
    }
}
