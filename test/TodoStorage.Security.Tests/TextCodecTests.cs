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
    using NUnit.Framework;

    [TestFixture]
    internal class TextCodecTests
    {
        private TextCodec sut;

        [SetUp]
        public void Setup()
        {
            sut = new TextCodec();
        }

        [Test]
        public void Decode_GivenNullValue_ThrowsException()
        {
            TestDelegate decodeCall =
                () => sut.Decode(null);

            Assert.That(decodeCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Decode_GivenValue_Base64DecodesTheValue()
        {
            var decodedValue = sut.Decode("VGhpcyBpcyBhbiBlbmNvZGVkIHN0cmluZyEhMQ==");

            Assert.That(decodedValue, Is.EqualTo("This is an encoded string!!1"));
        }

        [Test]
        public void Encode_GivenNullValue_ThrowsException()
        {
            TestDelegate encodeCall =
                () => sut.Encode(null);

            Assert.That(encodeCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Encode_GivenValue_Base64EncodesTheValue()
        {
            var encodedValue = sut.Encode("This is an encoded string!!1");

            Assert.That(encodedValue, Is.EqualTo("VGhpcyBpcyBhbiBlbmNvZGVkIHN0cmluZyEhMQ=="));
        }
    }
}
