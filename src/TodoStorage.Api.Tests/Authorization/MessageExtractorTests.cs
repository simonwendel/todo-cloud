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
    using System.Net.Http.Headers;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;
    using TodoStorage.Security;

    [TestFixture]
    internal class MessageExtractorTests
    {
        private MessageExtractor sut;

        private Mock<IMessageFactory> messageFactory;

        private IMessage message;

        [SetUp]
        public void Setup()
        {
            message = Mock.Of<IMessage>();

            messageFactory = new Mock<IMessageFactory>();
            messageFactory
                .Setup(f => f.Build(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<Uri>(),
                    It.IsAny<ulong>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(message);

            sut = new MessageExtractor(messageFactory.Object);
        }

        [Test]
        public void Ctor_GivenNullMessageFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new MessageExtractor(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void ExtractMessage_GivenNullActionContext_ThrowsException()
        {
            TestDelegate extractCall =
                () => sut.ExtractMessage(null);

            Assert.That(extractCall, Throws.ArgumentNullException);
        }

        [Test]
        public void ExtractMessage_GivenWrongAuthScheme_ThrowsException()
        {
            var context = new FakeHttpActionContext();
            context.Request.Headers.Authorization = new AuthenticationHeaderValue("nope");

            TestDelegate extractCall =
                () => sut.ExtractMessage(context);

            Assert.That(extractCall, Throws.ArgumentException);
        }

        [Test]
        public void ExtractMessage_GivenActionContext_ExtractsMessage()
        {
            var extractedMessage = sut.ExtractMessage(new FakeHttpActionContext());

            Assert.That(extractedMessage, Is.SameAs(message));
            messageFactory.Verify(
                f => f.Build(
                    It.Is<Guid>(g => g == FakeHttpActionContext.FakeAppId),
                    It.Is<string>(s => s.Equals(FakeHttpActionContext.FakeMethod.Method)),
                    It.Is<Uri>(u => u == FakeHttpActionContext.FakeUri),
                    It.Is<ulong>(t => t == FakeHttpActionContext.FakeTimestamp),
                    It.Is<string>(n => n.Equals(FakeHttpActionContext.FakeNonce)),
                    It.Is<string>(b => b.Equals(FakeHttpActionContext.FakeContent)),
                    It.Is<string>(s => s.Equals(FakeHttpActionContext.FakeSignature))),
                Times.Once);
        }
    }
}
