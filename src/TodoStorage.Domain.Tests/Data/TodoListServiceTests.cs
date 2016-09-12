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

namespace TodoStorage.Domain.Tests.Data
{
    using System;
    using Domain.Data;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class TodoListServiceTests
    {
        private TodoListService sut;

        private Mock<ITodoListRepository> mockRepository;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<ITodoListRepository>();
            sut = new TodoListService(mockRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullTodoRepository_ThrowsException()
        {
            TestDelegate constructorCall = 
                () => new TodoListService(null);

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
        public void GetList_GivenCollectionKey_ReturnsFromRepository()
        {
            var collectionKey = new CollectionKey(Guid.NewGuid());
            var expectedList = new TodoList(collectionKey, new Todo[0]);

            mockRepository
                .Setup(r => r.GetList(It.Is<CollectionKey>(key => key.Equals(collectionKey))))
                .Returns(expectedList);

            var actualList = sut.GetList(collectionKey);

            Assert.That(actualList, Is.SameAs(expectedList));
            mockRepository.VerifyAll();
        }
    }
}
