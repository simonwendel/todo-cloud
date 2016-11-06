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
    using Domain;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

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

        private TodoList sut;

        private TodoList sameProperties;

        private TodoList someDiffering;

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
            TestDelegate constructorCall =
                () => new TodoList(null, key);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullListKey_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoList(todoService.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenListKey_ConstructsListWithKey()
        {
            Assert.That(sut.Key, Is.EqualTo(key));
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
            Assert.That(sut.Items, Is.EquivalentTo(todos));
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            Assert.That(sut.Equals(someDiffering), Is.False);
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

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
