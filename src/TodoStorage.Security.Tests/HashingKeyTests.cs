﻿/*
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

namespace TodoStorage.Security.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TodoStorage.Security;

    [TestFixture]
    internal class HashingKeyTests
    {
        private HashingKey sut;

        private byte[] secret;

        private byte[] otherSecret;

        private Guid appId;

        private Guid otherAppId;

        private byte[] hash;

        private byte[] otherHash;

        private Mock<IMessageHasher> hasher;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            secret = fixture.CreateMany<byte>().ToArray();
            otherSecret = fixture.CreateMany<byte>().ToArray();

            appId = Guid.NewGuid();
            otherAppId = Guid.NewGuid();

            hash = fixture.CreateMany<byte>().ToArray();
            otherHash = fixture.CreateMany<byte>().ToArray();

            hasher = new Mock<IMessageHasher>();

            sut = new HashingKey(hasher.Object, appId, secret);
        }

        [Test]
        public void Ctor_GivenNullHasher_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(null, Guid.NewGuid(), secret);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(hasher.Object, Guid.Empty, new byte[100]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenNullSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(hasher.Object, Guid.NewGuid(), null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenZeroLengthSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(hasher.Object, Guid.NewGuid(), new byte[0]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Verify_GivenNullMessage_ThrowsException()
        {
            TestDelegate verifyCall =
                () => sut.Verify(null);

            Assert.That(verifyCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Verify_WhenHasherProducesDifferentHash_ReturnsFalse()
        {
            var message = new Message(appId, "some message", hash);

            hasher
                .Setup(h => h.HashMessage(It.Is<string>(m => m.Equals("some message"))))
                .Returns(otherHash);

            var hashesMatch = sut.Verify(message);

            Assert.That(hashesMatch, Is.False);
            hasher.Verify(
                h => h.HashMessage(It.Is<string>(m => m.Equals("some message"))),
                Times.Once);
        }

        [Test]
        public void Verify_WhenHasherProducesSameHash_ReturnsTrue()
        {
            var message = new Message(appId, "the message", hash);

            hasher
                .Setup(h => h.HashMessage(It.Is<string>(m => m.Equals("the message"))))
                .Returns(hash);

            var hashesMatch = sut.Verify(message);

            Assert.That(hashesMatch, Is.True);
            hasher.Verify(
                h => h.HashMessage(It.Is<string>(m => m.Equals("the message"))),
                Times.Once);
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new HashingKey(hasher.Object, appId, secret);

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new HashingKey(hasher.Object, appId, secret);
            var sameProperties = new HashingKey(hasher.Object, appId, secret);

            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new HashingKey(hasher.Object, appId, secret);

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new HashingKey(hasher.Object, appId, secret);
            var differingAppId = new HashingKey(hasher.Object, otherAppId, secret);
            var differingSecret = new HashingKey(hasher.Object, appId, otherSecret);

            Assert.That(sut.Equals(differingAppId), Is.False);
            Assert.That(sut.Equals(differingSecret), Is.False);
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = new HashingKey(hasher.Object, appId, secret);

            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + appId.GetHashCode();
                foreach (var b in secret)
                {
                    hash = (hash * multiplier) + b;
                }
            }

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
