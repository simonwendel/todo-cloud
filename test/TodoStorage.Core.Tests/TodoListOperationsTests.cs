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

    /// <summary>
    /// Unit tests the more domain-centric list operations for the <see cref="TodoList"/> class.
    /// </summary>
    [TestFixture]
    internal class TodoListOperationsTests
    {
        private Mock<ITodoService> todoService;
        private CollectionKey key;
        private IList<Todo> todos;
        private Todo someTodo;
        private TodoList sut;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            key = fixture.Create<CollectionKey>();
            todos = fixture.CreateMany<Todo>().ToList();
            someTodo = fixture.Create<Todo>();

            todoService = new Mock<ITodoService>();
            todoService.Setup(s => s.GetAll(key)).Returns(todos);

            sut = new TodoList(todoService.Object, key);
        }

        [Test]
        public void Add_GivenNullTodo_ThrowsException()
        {
            Action adding = () => sut.Add(null);
            adding.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Add_GivenTodo_AddsViaTodoService()
        {
            sut.Add(someTodo);
            todoService.Verify(s => s.Add(someTodo, key),Times.Once);
        }

        [Test]
        public void Add_GivenTodo_RefreshesFromTodoService()
        {
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Once);
            sut.Add(someTodo);
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Exactly(2));
        }

        [Test]
        public void Update_GivenNullTodo_ThrowsException()
        {
            Action updating = () => sut.Update(null);
            updating.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Update_GivenTodo_UpdatesViaTodoService()
        {
            sut.Update(someTodo);
            todoService.Verify(s => s.Update(someTodo, key), Times.Once);
        }

        [Test]
        public void Update_GivenTodo_RefreshesFromTodoService()
        {
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Once);
            sut.Update(someTodo);
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Exactly(2));
        }

        [Test]
        public void Delete_GivenNullTodo_ThrowsException()
        {
            Action deleting = () => sut.Delete(null);
            deleting.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Delete_GivenTodo_DeletesViaTodoService()
        {
            sut.Delete(someTodo);
            todoService.Verify(s => s.Delete(someTodo, key), Times.Once);
        }

        [Test]
        public void Delete_GivenTodo_RefreshesFromTodoService()
        {
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Once);
            sut.Delete(someTodo);
            todoService.Verify(s => s.GetAll(It.IsAny<CollectionKey>()), Times.Exactly(2));
        }
    }
}
