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

namespace TodoStorage.Security.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class MessageFactoryTests
    {
        private MessageFactory sut;

        private Mock<IHashConverter> hashConverter;

        [SetUp]
        public void Setup()
        {
            hashConverter = new Mock<IHashConverter>();

            hashConverter
                .Setup(c => c.HexToBytes(It.Is<string>(s => s.Equals(MessageFactoryTestsData.Signature))))
                .Returns(MessageFactoryTestsData.Hash);

            sut = new MessageFactory(hashConverter.Object);
        }

        [Test]
        public void Ctor_GivenNullHashConverter_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new MessageFactory(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [TestCaseSource(typeof(MessageFactoryTestsData), "NullParameterCases")]
        public void Build_GivenNullParameter_ThrowsException(Guid appId, string method, Uri uri, ulong timestamp, string nonce, string body, string signature)
        {
            TestDelegate buildCall =
                () => sut.Build(appId, method, uri, timestamp, nonce, body, signature);

            Assert.That(buildCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Build_GivenAllParameters_ConvertsSignatureToArray()
        {
            sut.Build(
                MessageFactoryTestsData.AppId,
                MessageFactoryTestsData.Method,
                MessageFactoryTestsData.Uri,
                MessageFactoryTestsData.Timestamp,
                MessageFactoryTestsData.Nonce,
                MessageFactoryTestsData.Body,
                MessageFactoryTestsData.Signature);

            hashConverter.Verify(
                c => c.HexToBytes(It.Is<string>(s => s.Equals(MessageFactoryTestsData.Signature))), 
                Times.Once);
        }

        [Test]
        public void Build_GivenAllParameters_ConstructsMessage()
        {
            var expected = new Message(
                MessageFactoryTestsData.AppId,
                MessageFactoryTestsData.Method,
                MessageFactoryTestsData.Uri.PathAndQuery,
                MessageFactoryTestsData.Timestamp,
                MessageFactoryTestsData.Nonce,
                MessageFactoryTestsData.Body,
                MessageFactoryTestsData.Hash);

            var message = sut.Build(
                MessageFactoryTestsData.AppId,
                MessageFactoryTestsData.Method,
                MessageFactoryTestsData.Uri,
                MessageFactoryTestsData.Timestamp,
                MessageFactoryTestsData.Nonce,
                MessageFactoryTestsData.Body,
                MessageFactoryTestsData.Signature);

            Assert.That(message, Is.EqualTo(expected));
        }

        private static class MessageFactoryTestsData
        {
            static MessageFactoryTestsData()
            {
                var fixture = new Fixture();

                AppId = fixture.Create<Guid>();
                Method = fixture.Create<string>();
                Uri = fixture.Create<Uri>();
                Timestamp = fixture.Create<ulong>();
                Nonce = fixture.Create<string>();
                Body = fixture.Create<string>();
                Signature = fixture.Create<string>();
                Hash = fixture.CreateMany<byte>().ToArray();
            }

            public static Guid AppId { get; private set; }

            public static string Method { get; private set; }

            public static Uri Uri { get; private set; }

            public static ulong Timestamp { get; private set; }

            public static string Nonce { get; private set; }

            public static string Body { get; private set; }

            public static string Signature { get; private set; }

            public static byte[] Hash { get; private set; }

            public static object[] NullParameterCases => new[]
            {
                new object[] { AppId, null, Uri, Timestamp, Nonce, Body, Signature },
                new object[] { AppId, Method, null, Timestamp, Nonce, Body, Signature },
                new object[] { AppId, Method, Uri, Timestamp, null, Body, Signature },
                new object[] { AppId, Method, Uri, Timestamp, Nonce, null, Signature },
                new object[] { AppId, Method, Uri, Timestamp, Nonce, Body, null }
            };
        }
    }
}
