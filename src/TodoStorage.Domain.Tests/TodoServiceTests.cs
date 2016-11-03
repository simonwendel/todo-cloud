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

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            collectionKey = fixture.Create<CollectionKey>();
            listOfTodos = fixture.CreateMany<Todo>().ToList();

            todoRepository = new Mock<ITodoRepository>();
            todoRepository
                .Setup(r => r.GetAll(It.IsAny<CollectionKey>()))
                .Returns(listOfTodos);

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
    }
}
