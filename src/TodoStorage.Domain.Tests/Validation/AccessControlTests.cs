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

namespace TodoStorage.Domain.Tests.Validation
{
    using Domain.Data;
    using Domain.Validation;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class AccessControlTests
    {
        private CollectionKey key;

        private AccessControl sut;

        private Mock<IAccessControlRepository> mockRepository;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            key = fixture.Create<CollectionKey>();

            mockRepository = new Mock<IAccessControlRepository>();

            sut = new AccessControl(mockRepository.Object);
        }

        [Test]
        public void Ctor_GivenNullAccessControlRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AccessControl(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_GivenNullCollectionKey_ThrowsException()
        {
            TestDelegate methodCall =
                () => sut.IsOwnerOf(null, 18);

            Assert.That(methodCall, Throws.ArgumentNullException);
        }

        [Test]
        public void IsOwnerOf_IfOwnerFromRepository_ReturnsTrue()
        {
            SetupIfOwner(true);

            Assert.That(sut.IsOwnerOf(key, 1227), Is.True);
            mockRepository.VerifyAll();
        }

        [Test]
        public void IsOwnerOf_IfNotOwnerFromRepository_ReturnsFalse()
        {
            SetupIfOwner(false);

            Assert.That(sut.IsOwnerOf(key, 1227), Is.False);
            mockRepository.VerifyAll();
        }

        private void SetupIfOwner(bool keyIsOwner)
        {
            mockRepository
                .Setup(repo => repo.IsOwnerOf(It.IsAny<CollectionKey>(), It.IsAny<int>()))
                .Returns(keyIsOwner);
        }
    }
}
