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
    using TodoStorage.Core;

    /// <summary>
    /// Unit tests basic <see cref="TodoList"/> object creation through constructor tests and 
    /// tests for the Object overrides on that class.
    /// </summary>
    [TestFixture]
    internal class TodoListObjectTests
    {
        private Mock<ITodoService> todoService;
        private CollectionKey key;
        private IList<Todo> todos;
        private TodoList sameProperties;
        private TodoList someDiffering;
        private TodoList sut;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            key = fixture.Create<CollectionKey>();
            var otherKey = fixture.Create<CollectionKey>();

            todos = fixture.CreateMany<Todo>().ToList();
            var otherTodos = fixture.CreateMany<Todo>().ToList();

            todoService = new Mock<ITodoService>();

            todoService
                .Setup(s => s.GetAll(It.Is<CollectionKey>(k => k == key)))
                .Returns(todos);

            todoService
                .Setup(s => s.GetAll(It.Is<CollectionKey>(k => k == otherKey)))
                .Returns(otherTodos);

            sut = new TodoList(todoService.Object, key);

            sameProperties = new TodoList(todoService.Object, key);
            someDiffering = new TodoList(todoService.Object, otherKey);
        }

        [Test]
        public void Ctor_GivenNullTodoService_ThrowsException()
        {
            Action constructing = () => new TodoList(null, key);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Ctor_GivenNullListKey_ThrowsException()
        {
            Action constructing = () => new TodoList(todoService.Object, null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Ctor_GivenListKey_ConstructsListWithKey()
        {
            sut.Key.Should().Be(key);
        }

        [Test]
        public void Ctor_GivenTodoService_CallsTodoService()
        {
            todoService.Verify(
                s => s.GetAll(It.Is<CollectionKey>(k => k == key)),
                Times.AtLeastOnce);
        }

        [Test]
        public void Ctor_GivenTodoService_PopulatesTheList()
        {
            sut.Items.Should().BeEquivalentTo(todos);
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            sut.Equals(sut).Should().BeTrue();
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            sut.Equals(sameProperties).Should().BeTrue();
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            sut.Equals(null).Should().BeFalse();
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            sut.Equals(someDiffering).Should().BeFalse();
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                var seed = (start * multiplier) + sut.Key.GetHashCode();
                hash = sut.Items.Aggregate(
                    seed,
                    (h, todo) => unchecked((h * multiplier) + todo.GetHashCode()));
            }

            sut.GetHashCode().Should().Be(hash);
        }
    }
}
