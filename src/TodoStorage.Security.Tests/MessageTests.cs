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

        private string method;

        private string path;

        private ulong timestamp;

        private string nonce;

        private string body;

        private byte[] signature;

        private Guid otherAppId;

        private string otherMethod;

        private string otherPath;

        private ulong otherTimestamp;

        private string otherNonce;

        private string otherBody;

        private byte[] otherSignature;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            appId = fixture.Create<Guid>();
            otherAppId = fixture.Create<Guid>();

            method = fixture.Create<string>();
            otherMethod = fixture.Create<string>();

            path = fixture.Create<string>();
            otherPath = fixture.Create<string>();

            timestamp = fixture.Create<ulong>();
            otherTimestamp = fixture.Create<ulong>();

            nonce = fixture.Create<string>();
            otherNonce = fixture.Create<string>();

            body = fixture.Create<string>();
            otherBody = fixture.Create<string>();

            signature = fixture.CreateMany<byte>().ToArray();
            otherSignature = fixture.CreateMany<byte>().ToArray();
        }

        [Test]
        public void Ctor_GivenEmptyAppId_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(Guid.Empty, method, path, timestamp, nonce, body, signature);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenNullMethod_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, null, path, timestamp, nonce, body, signature);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullPath_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, method, null, timestamp, nonce, body, signature);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullNonce_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, method, path, timestamp, null, body, signature);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullBody_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, method, path, timestamp, nonce, null, signature);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullSignature_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new Message(appId, method, path, timestamp, nonce, body, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenParameters_SetsProperties()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);

            Assert.That(sut.AppId, Is.EqualTo(appId));
            Assert.That(sut.Method, Is.EqualTo(method));
            Assert.That(sut.Path, Is.EqualTo(path));
            Assert.That(sut.Timestamp, Is.EqualTo(timestamp));
            Assert.That(sut.Nonce, Is.EqualTo(nonce));
            Assert.That(sut.Body, Is.EqualTo(body));
            Assert.That(sut.Signature, Is.EquivalentTo(signature));
        }

        [Test]
        public void ToString_ProducesColonSeparatedConcatenation()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);
            var expected = $"{appId.ToString()}:{method}:{path}:{timestamp}:{nonce}:{body}";

            Assert.That(sut.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);
            var sameProperties = new Message(appId, method, path, timestamp, nonce, body, signature);

            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);

            var differingAppId = new Message(otherAppId, method, path, timestamp, nonce, body, signature);
            var differingMethod = new Message(appId, otherMethod, path, timestamp, nonce, body, signature);
            var differingPath = new Message(appId, method, otherPath, timestamp, nonce, body, signature);
            var differingTimestamp = new Message(appId, method, path, otherTimestamp, nonce, body, signature);
            var differingNonce = new Message(appId, method, path, timestamp, otherNonce, body, signature);
            var differingBody = new Message(appId, method, path, timestamp, nonce, otherBody, signature);
            var differingSignature = new Message(appId, method, path, timestamp, nonce, body, otherSignature);

            Assert.That(sut.Equals(differingAppId), Is.False);
            Assert.That(sut.Equals(differingMethod), Is.False);
            Assert.That(sut.Equals(differingPath), Is.False);
            Assert.That(sut.Equals(differingTimestamp), Is.False);
            Assert.That(sut.Equals(differingNonce), Is.False);
            Assert.That(sut.Equals(differingBody), Is.False);
            Assert.That(sut.Equals(differingSignature), Is.False);
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = new Message(appId, method, path, timestamp, nonce, body, signature);

            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + appId.GetHashCode();
                hash = (hash * multiplier) + method.GetHashCode();
                hash = (hash * multiplier) + path.GetHashCode();
                hash = (hash * multiplier) + timestamp.GetHashCode();
                hash = (hash * multiplier) + nonce.GetHashCode();
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
