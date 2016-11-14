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

namespace TodoStorage.Security.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Security;

    [TestFixture]
    internal class HashingKeyTests
    {
        private byte[] secret;

        private byte[] otherSecret;

        private Guid appId;

        private Guid otherAppId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            secret = fixture.CreateMany<byte>().ToArray();
            otherSecret = fixture.CreateMany<byte>().ToArray();

            appId = Guid.NewGuid();
            otherAppId = Guid.NewGuid();
        }

        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(Guid.Empty, new byte[100]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenNullSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(System.Guid.NewGuid(), null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenZeroLengthSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HashingKey(System.Guid.NewGuid(), new byte[0]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new HashingKey(appId, secret);

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new HashingKey(appId, secret);
            var sameProperties = new HashingKey(appId, secret);

            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new HashingKey(appId, secret);

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new HashingKey(appId, secret);
            var differingAppId = new HashingKey(otherAppId, secret);
            var differingSecret = new HashingKey(appId, otherSecret);

            Assert.That(sut.Equals(differingAppId), Is.False);
            Assert.That(sut.Equals(differingSecret), Is.False);
        }
    }
}
