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
    using System.Net;
    using System.Web.Http.Controllers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;
    using TodoStorage.Security;

    /// <remarks>
    /// Fun fact about this test class is that all the authentication logic is done in the protected 
    /// IsAuthorized method on the attribute under test, but it is reached through the OnAuthorization
    /// on the base class. Therefore the <see cref="FakeHttpActionContext"/> class has to be quite 
    /// detailed.
    /// </remarks>
    [TestFixture]
    internal class KeyAuthorizeAttributeTests
    {
        private KeyAuthorizeAttribute sut;

        private Mock<IHashingKeyFactory> keyFactory;

        private Mock<IHashingKey> hashingKey;

        private Mock<IMessageExtractor> messageExtractor;

        private HttpActionContext action;

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

            action = new FakeHttpActionContext();

            sut = new KeyAuthorizeAttribute(keyFactory.Object, messageExtractor.Object);
        }

        [Test]
        public void Ctor_GivenNullHashingKeyFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizeAttribute(null, messageExtractor.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullMessageExtractor_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new KeyAuthorizeAttribute(keyFactory.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void OnAuthorization_GivenActionContext_ExtractsMessage()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(true);

            sut.OnAuthorization(action);

            messageExtractor.Verify(
                e => e.ExtractMessage(It.Is<HttpActionContext>(a => a == action)),
                Times.Once);
        }

        [Test]
        public void OnAuthorization_WhenKeyFactoryThrowsException_DeniesTheRequest()
        {
            keyFactory
                .Setup(f => f.Build(It.IsAny<Guid>()))
                .Throws(new KeyNotFoundException());

            sut.OnAuthorization(action);

            Assert.That(action.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            keyFactory.Verify(
                f => f.Build(It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public void OnAuthorization_IfHashDoesntMatch_DeniesTheRequest()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(false);

            sut.OnAuthorization(action);

            Assert.That(action.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            hashingKey.Verify(
                k => k.Verify(It.IsAny<Message>()),
                Times.Once);
        }

        [Test]
        public void OnAuthorization_IfHashMatches_AllowsTheRequestToContinue()
        {
            hashingKey
                .Setup(k => k.Verify(It.IsAny<Message>()))
                .Returns(true);

            sut.OnAuthorization(action);

            Assert.That(action.Response.StatusCode, Is.Not.EqualTo(HttpStatusCode.Unauthorized));
            hashingKey.Verify(
                k => k.Verify(It.IsAny<Message>()),
                Times.Once);
        }
    }
}
