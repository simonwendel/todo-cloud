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

namespace TodoStorage.Api.Tests.Authorization
{
    using System;
    using System.Net.Http.Headers;
    using System.Web.Http.Controllers;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;
    using TodoStorage.Security;

    [TestFixture]
    internal class HmacAuthenticatorTests
    {
        private Mock<IHashingKeyFactory> keyFactory;

        private Mock<IHashingKey> key;

        private Mock<IMessageExtractor> messageExtractor;

        private Mock<IMessage> message;

        private FakeHttpAuthenticationContext context;

        private HmacAuthenticator sut;

        [SetUp]
        public void Setup()
        {
            message = new Mock<IMessage>();
            messageExtractor = new Mock<IMessageExtractor>();
            messageExtractor
                .Setup(e => e.ExtractMessage(It.IsAny<HttpActionContext>()))
                .Returns(message.Object);

            key = new Mock<IHashingKey>();
            keyFactory = new Mock<IHashingKeyFactory>();
            keyFactory
                .Setup(k => k.Build(It.IsAny<Guid>()))
                .Returns(key.Object);

            context = new FakeHttpAuthenticationContext();

            sut = new HmacAuthenticator(keyFactory.Object, messageExtractor.Object);
        }

        [Test]
        public void Ctor_GivenNullHashingKeyFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HmacAuthenticator(null, messageExtractor.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullMessageExtractor_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new HmacAuthenticator(keyFactory.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Authenticate_GivenNullContext_ThrowsException()
        {
            TestDelegate authenticateCall =
                () => sut.Authenticate(null);

            Assert.That(authenticateCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Authenticate_GivenNullAuthorizationHeaders_DoesNoOp()
        {
            context.Request.Headers.Authorization = null;

            sut.Authenticate(context);
        }

        [Test]
        public void Authenticate_GivenIncorrectAuthorizationScheme_DoesNoOp()
        {
            context.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "username:password");

            sut.Authenticate(context);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Authenticate_GivenEmptyAuthorizationParameter_DoesNoOp(string parameter)
        {
            context.Request.Headers.Authorization =
                new AuthenticationHeaderValue(ValidAuthenticationSchemes.Hmac, parameter);

            sut.Authenticate(context);
        }

        [Test]
        public void Authenticate_MessageExtractorThrowsException_ReturnsUnauthenticatedResponse()
        {
            messageExtractor
                .Setup(e => e.ExtractMessage(It.IsAny<HttpActionContext>()))
                .Throws(new BadMessageFormatException());

            sut.Authenticate(context);

            Assert.That(context.ErrorResult is AuthenticationFailureResult);
            Assert.That(context.Principal, Is.Null);

            messageExtractor.Verify(
                e => e.ExtractMessage(It.IsAny<HttpActionContext>()),
                Times.Once);
        }

        [Test]
        public void Authenticate_KeyFactoryThrowsException_ReturnsUnauthenticatedResponse()
        {
            keyFactory
                .Setup(k => k.Build(It.IsAny<Guid>()))
                .Throws(new KeyNotFoundException());

            sut.Authenticate(context);

            Assert.That(context.ErrorResult is AuthenticationFailureResult);
            Assert.That(context.Principal, Is.Null);

            keyFactory.Verify(
                k => k.Build(It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public void Authenticate_KeyVerificationFails_ReturnsUnauthenticatedResponse()
        {
            key
                .Setup(k => k.Verify(It.Is<IMessage>(m => m == message.Object)))
                .Returns(false);

            sut.Authenticate(context);

            Assert.That(context.ErrorResult is AuthenticationFailureResult);
            Assert.That(context.Principal, Is.Null);

            key.Verify(
                k => k.Verify(It.Is<IMessage>(m => m == message.Object)),
                Times.Once);
        }

        [Test]
        public void Authenticate_KeyVerificationSucceeds_SetsPrincipal()
        {
            key
                .Setup(k => k.Verify(It.Is<IMessage>(m => m == message.Object)))
                .Returns(true);

            sut.Authenticate(context);

            Assert.That(context.ErrorResult == null);
            Assert.That(context.Principal is SignedMessagePrincipal);

            key.Verify(
                k => k.Verify(It.Is<IMessage>(m => m == message.Object)),
                Times.Once);
        }
    }
}
