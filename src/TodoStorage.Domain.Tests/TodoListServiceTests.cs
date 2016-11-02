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

    [TestFixture]
    internal class TodoListServiceTests
    {
        private TodoListService sut;

        private Mock<ITodoRepository> mockRepository;

        private Mock<ITodoListFactory> mockFactory;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<ITodoRepository>();
            mockFactory = new Mock<ITodoListFactory>();
            sut = new TodoListService(mockRepository.Object, mockFactory.Object);
        }

        [Test]
        public void Ctor_GivenNullTodoRepository_ThrowsException()
        {
            TestDelegate constructorCall = 
                () => new TodoListService(null, mockFactory.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullTodoListFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoListService(mockRepository.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetList_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.GetList(null);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetList_GivenCollectionKey_ConstructsFromRepository()
        {
            var fixture = new Fixture();
            var collectionKey = fixture.Create<CollectionKey>();
            var todoItems = fixture.CreateMany<Todo>().ToList();

            var expected = new TodoList(collectionKey, todoItems);

            mockRepository
                .Setup(r => r.GetAll(It.Is<CollectionKey>(key => key.Equals(collectionKey))))
                .Returns(todoItems);

            mockFactory
                .Setup(f => f.Create(
                    It.Is<CollectionKey>(key => key.Equals(collectionKey)),
                    It.Is<IEnumerable<Todo>>(t => t.Equals(todoItems))))
                .Returns(expected);

            var actual = sut.GetList(collectionKey);

            Assert.That(actual, Is.EqualTo(expected));
            mockRepository.VerifyAll();
            mockFactory.VerifyAll();
        }
    }
}
