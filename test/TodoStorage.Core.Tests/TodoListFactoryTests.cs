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
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TodoStorage.Core;

    [TestFixture]
    internal class TodoListFactoryTests
    {
        private TodoListFactory sut;

        private Mock<ITodoService> todoService;

        [SetUp]
        public void Setup()
        {
            todoService = new Mock<ITodoService>();
            sut = new TodoListFactory(todoService.Object);
        }

        [Test]
        public void Ctor_GivenNullTodoService_ThrowsException()
        {
            TestDelegate constructorCall = 
                () => new TodoListFactory(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Create_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.Create(null);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Create_GivenCollectionKey_ReturnsList()
        {
            var fixture = new Fixture();
            var collectionKey = fixture.Create<CollectionKey>();
            var todoItems = fixture.CreateMany<Todo>().ToList();

            todoService
                .Setup(r => r.GetAll(It.Is<CollectionKey>(key => key.Equals(collectionKey))))
                .Returns(todoItems);

            var expected = new TodoList(todoService.Object, collectionKey);

            var actual = sut.Create(collectionKey);

            Assert.That(actual, Is.EqualTo(expected));
            todoService.VerifyAll();
        }
    }
}
