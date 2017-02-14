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
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Controllers;
    using TodoStorage.Domain;

    [TestFixture]
    internal class TodoControllerTests : ControllerTestFixtureBase
    {
        private Mock<ITodoListFactory> todoListFactory;

        private Mock<ITodoList> todoList;

        private TodoController sut;

        [SetUp]
        public void Setup()
        {
            SetPrincipal(Guid.NewGuid());

            todoListFactory = new Mock<ITodoListFactory>();
            todoList = new Mock<ITodoList>();

            todoListFactory
                .Setup(f => f.Create(It.IsAny<CollectionKey>()))
                .Returns(todoList.Object);
        }

        [Test]
        public void Ctor_GivenNullTodoListFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new TodoController(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenTodoListFactory_RetrievesTodoList()
        {
            sut = new TodoController(todoListFactory.Object);

            todoListFactory.Verify(
                f => f.Create(It.IsAny<CollectionKey>()),
                Times.Once);
        }
    }
}
