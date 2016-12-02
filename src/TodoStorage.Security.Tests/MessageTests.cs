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

    [TestFixture]
    internal class MessageTests
    {
        private Guid appId;

        private string body;

        private byte[] signature;

        private Guid otherAppId;

        private string otherBody;

        private byte[] otherSignature;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            appId = fixture.Create<Guid>();
            otherAppId = fixture.Create<Guid>();

            body = fixture.Create<string>();
            otherBody = fixture.Create<string>();

            signature = fixture.CreateMany<byte>().ToArray();
            otherSignature = fixture.CreateMany<byte>().ToArray();
        }

        [Test]
        public void Ctor_GivenEmptyAppId_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(Guid.Empty, body, signature);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenNullBody_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, null, signature);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullSignature_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, body, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenParameters_SetsProperties()
        {
            var sut = new Message(appId, body, signature);

            Assert.That(sut.AppId, Is.EqualTo(appId));
            Assert.That(sut.Body, Is.EqualTo(body));
            Assert.That(sut.Signature, Is.EquivalentTo(signature));
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new Message(appId, body, signature);

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new Message(appId, body, signature);
            var sameProperties = new Message(appId, body, signature);

            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new Message(appId, body, signature);

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new Message(appId, body, signature);
            var differingAppId = new Message(otherAppId, body, signature);
            var differingBody = new Message(appId, otherBody, signature);
            var differingSignature = new Message(appId, body, otherSignature);

            Assert.That(sut.Equals(differingAppId), Is.False);
            Assert.That(sut.Equals(differingBody), Is.False);
            Assert.That(sut.Equals(differingSignature), Is.False);
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = new Message(appId, body, signature);

            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + appId.GetHashCode();
                hash = (hash * multiplier) + body.GetHashCode();
                foreach (var b in signature)
                {
                    hash = (hash * multiplier) + b;
                }
            }

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
