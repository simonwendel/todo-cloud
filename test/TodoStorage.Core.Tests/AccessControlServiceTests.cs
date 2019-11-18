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
    using AutoFixture;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Core;

    [TestFixture]
    internal class AccessControlServiceTests
    {
        private CollectionKey key;

        private Todo todo;

        private AccessControlService sut;

        private Mock<IAccessControlRepository> accessControlRepository;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            key = fixture.Create<CollectionKey>();
            todo = fixture.Create<Todo>();

            accessControlRepository = new Mock<IAccessControlRepository>();

            sut = new AccessControlService(accessControlRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullAccessControlRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AccessControlService(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.IsOwnerOf(null, todo);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_IfTodoHasNullId_ReturnsFalse()
        {
            var notPersistedTodo = new Todo
            {
                Id = null
            };

            Assert.That(sut.IsOwnerOf(key, notPersistedTodo), Is.False);
        }

        [Test]
        public void IsOwnerOf_IfOwnerFromRepository_ReturnsTrue()
        {
            SetupIfOwner(true);

            Assert.That(sut.IsOwnerOf(key, todo), Is.True);
            accessControlRepository.VerifyAll();
        }

        [Test]
        public void IsOwnerOf_IfNotOwnerFromRepository_ReturnsFalse()
        {
            SetupIfOwner(false);

            Assert.That(sut.IsOwnerOf(key, todo), Is.False);
            accessControlRepository.VerifyAll();
        }

        private void SetupIfOwner(bool keyIsOwner)
        {
            accessControlRepository
                .Setup(repo => repo.IsOwnerOf(It.IsAny<CollectionKey>(), It.Is<int>(i => i == todo.Id)))
                .Returns(keyIsOwner);
        }
    }
}
