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
    using System.Security.Cryptography;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    internal class MessageHasherTests : IDisposable
    {
        private MessageHasher sut;

        private HMACSHA256 algorithm;

        private byte[] key;

        private string message;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            message = fixture.Create<string>();
            key = fixture.CreateMany<byte>(32).ToArray();

            algorithm = new HMACSHA256(key);

            sut = new MessageHasher(algorithm);
        }

        [Test]
        public void Ctor_GivenNullAlgorithm_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new MessageHasher(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void HashMessage_GivenNullMessage_ThrowsException()
        {
            TestDelegate hashMessageCall =
                () => sut.HashMessage(null);

            Assert.That(hashMessageCall, Throws.ArgumentNullException);
        }

        [Test]
        public void HashMessage_GivenMessageAndKey_ComnputesHash()
        {
            var messageBytes = EncodingOption.Default.GetBytes(message);
            var expectedHash = algorithm.ComputeHash(messageBytes);

            var hash = sut.HashMessage(message);

            Assert.That(hash, Is.EquivalentTo(expectedHash));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (algorithm != null)
                {
                    algorithm.Dispose();
                    algorithm = null;
                }
            }
        }
    }
}
