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

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            appId = fixture.Create<Guid>();
            body = fixture.Create<string>();
            signature = fixture.CreateMany<byte>().ToArray();
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
    }
}
