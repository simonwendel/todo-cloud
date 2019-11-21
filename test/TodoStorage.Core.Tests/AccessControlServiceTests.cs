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
    using AutoFixture;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Core;

    [TestFixture]
    internal class AccessControlServiceTests
    {
        private CollectionKey key;
        private Todo todo;
        private Mock<IAccessControlRepository> accessControlRepository;
        private AccessControlService sut;

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
            Action constructing = () => new AccessControlService(null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsOwnerOf_GivenNullCollectionKey_ThrowsException()
        {
            Action checkingIfOwner = () => sut.IsOwnerOf(null, todo);
            checkingIfOwner.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsOwnerOf_IfTodoHasNullId_ReturnsFalse()
        {
            var notPersistedTodo = new Todo
            {
                Id = null
            };

            sut.IsOwnerOf(key, notPersistedTodo).Should().BeFalse();
        }

        [Test]
        public void IsOwnerOf_IfOwnerFromRepository_ReturnsTrue()
        {
            SetupIfOwner(true);

            sut.IsOwnerOf(key, todo).Should().BeTrue();
            accessControlRepository.VerifyAll();
        }

        [Test]
        public void IsOwnerOf_IfNotOwnerFromRepository_ReturnsFalse()
        {
            SetupIfOwner(false);

            sut.IsOwnerOf(key, todo).Should().BeFalse();
            accessControlRepository.VerifyAll();
        }

        private void SetupIfOwner(bool keyIsOwner)
        {
            accessControlRepository
                .Setup(repo => repo.IsOwnerOf(It.IsAny<CollectionKey>(), todo.Id.Value))
                .Returns(keyIsOwner);
        }
    }
}
