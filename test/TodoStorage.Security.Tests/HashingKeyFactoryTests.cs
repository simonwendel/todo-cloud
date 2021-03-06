﻿/*
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

namespace TodoStorage.Security.Tests
{
    using System;
    using System.Linq;
    using AutoFixture;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class HashingKeyFactoryTests
    {
        private HashingKeyFactory sut;

        private Guid appId;

        private Guid otherAppId;

        private byte[] secret;

        private Mock<IAuthenticationRepository> authenticationRepository;

        private IMessageHasher messageHasher;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            appId = Guid.NewGuid();
            otherAppId = Guid.NewGuid();
            secret = fixture.CreateMany<byte>().ToArray();

            authenticationRepository = new Mock<IAuthenticationRepository>();

            authenticationRepository
                .Setup(r => r.GetSecret(It.Is<Guid>(g => g == appId)))
                .Returns(secret);

            authenticationRepository
                .Setup(r => r.GetSecret(It.Is<Guid>(g => g == otherAppId)))
                .Returns<byte[]>(null);

            messageHasher = Mock.Of<IMessageHasher>();

            sut = new HashingKeyFactory(authenticationRepository.Object, messageHasher);
        }

        [Test]
        public void Ctor_GivenNullRepository_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKeyFactory(null, messageHasher);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullHasher_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKeyFactory(authenticationRepository.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Build_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate createCall =
                () => sut.Build(Guid.Empty);

            Assert.That(createCall, Throws.ArgumentException);
            authenticationRepository.Verify(
                r => r.GetSecret(It.IsAny<Guid>()),
                Times.Never);
        }

        [Test]
        public void Build_GivenNonExistentGuid_ThrowsException()
        {
            TestDelegate createCall = 
                () => sut.Build(otherAppId);

            Assert.That(createCall, Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void Build_GivenGuid_CallsRepository()
        {
            sut.Build(appId);

            authenticationRepository.Verify(
                r => r.GetSecret(It.Is<Guid>(g => g == appId)),
                Times.Once);
        }

        [Test]
        public void Build_GivenGuid_ConstructsWithSecret()
        {
            var expected = new HashingKey(Mock.Of<IMessageHasher>(), appId, secret);

            var actual = sut.Build(appId);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
