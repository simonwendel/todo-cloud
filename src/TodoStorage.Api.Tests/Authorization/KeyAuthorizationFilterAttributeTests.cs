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

namespace TodoStorage.Api.Tests.Authorization
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;
    using TodoStorage.Security;

    [TestFixture]
    internal class KeyAuthorizationFilterAttributeTests
    {
        private KeyAuthorizationFilterAttribute sut;

        private Mock<IHashingKeyFactory> keyFactory;

        private Mock<IHashingKey> hashingKey;

        private Mock<IMessageExtractor> messageExtractor;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            var message = fixture.Create<Message>();

            messageExtractor = new Mock<IMessageExtractor>();
            messageExtractor
                .Setup(e => e.ExtractMessage(It.IsAny<HttpActionContext>()))
                .Returns(message);

            hashingKey = new Mock<IHashingKey>();

            keyFactory = new Mock<IHashingKeyFactory>();
            keyFactory
                .Setup(f => f.Build(It.IsAny<Guid>()))
                .Returns(hashingKey.Object);

            sut = new KeyAuthorizationFilterAttribute(keyFactory.Object, messageExtractor.Object);
        }

        [Test]
        public void Ctor_GivenNullHashingKeyFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizationFilterAttribute(null, messageExtractor.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullMessageExtractor_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizationFilterAttribute(keyFactory.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void OnAuthorization_GivenNullActionContext_ThrowsException()
        {
            TestDelegate authorizationCall =
                () => sut.OnAuthorization(null);

            Assert.That(authorizationCall, Throws.ArgumentNullException);
        }

        [Test]
        public void OnAuthorization_GivenActionContext_ExtractsMessage()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(true);

            sut.OnAuthorization(new FakeHttpActionContext());

            messageExtractor.Verify(
                e => e.ExtractMessage(It.IsAny<HttpActionContext>()), 
                Times.Once);
        }

        [Test]
        public void OnAuthorization_WhenKeyFactoryThrowsException_ThrowsExeption()
        {
            keyFactory
                .Setup(f => f.Build(It.IsAny<Guid>()))
                .Throws(new KeyNotFoundException());

            TestDelegate authorizationCall =
                () => sut.OnAuthorization(new FakeHttpActionContext());

            Assert.That(authorizationCall, Throws.TypeOf<HttpResponseException>());
            keyFactory.Verify(
                f => f.Build(It.IsAny<Guid>()), 
                Times.Once);
        }

        [Test]
        public void OnAuthorization_IfHashDoesntMatch_ThrowsException()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(false);

            TestDelegate authorizationCall =
                () => sut.OnAuthorization(new FakeHttpActionContext());

            Assert.That(authorizationCall, Throws.TypeOf<HttpResponseException>());
            hashingKey.Verify(
                k => k.Verify(It.IsAny<Message>()), 
                Times.Once);
        }

        [Test]
        public void OnAuthorization_IfHashMatches_DoesNothing()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(true);

            sut.OnAuthorization(new FakeHttpActionContext());

            hashingKey.Verify(
                k => k.Verify(It.IsAny<Message>()),
                Times.Once);
        }
    }
}
