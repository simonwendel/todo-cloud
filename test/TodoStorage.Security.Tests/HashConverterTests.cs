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
    internal class HashConverterTests
    {
        private HashConverter sut;

        private byte[] fibonacci;

        private string fibonacciString;

        private string fibonacciPrepended;

        [SetUp]
        public void Setup()
        {
            fibonacci = new byte[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233 };

            fibonacciString = "000101020305080d1522375990e9";
            fibonacciPrepended = "0x000101020305080d1522375990e9";

            sut = new HashConverter();
        }

        [Test]
        public void HexToBytes_GivenNullHexString_ThrowsException()
        {
            TestDelegate conversionCall =
                () => sut.HexToBytes(null);

            Assert.That(conversionCall, Throws.ArgumentNullException);
        }

        [Test]
        public void HexToBytes_GivenOddLengthHexString_ThrowsException()
        {
            TestDelegate conversionCall =
                () => sut.HexToBytes("aaa");

            Assert.That(conversionCall, Throws.ArgumentException);
        }

        [Test]
        public void HexToBytes_GivenEmptyHexString_ReturnsEmptyArray()
        {
            var bytes = sut.HexToBytes(string.Empty);

            Assert.That(bytes, Is.EquivalentTo(new byte[0]));
        }

        [Test]
        public void HexToBytes_GivenHexString_ReturnsByteArray()
        {
            var bytes = sut.HexToBytes(fibonacciString);

            Assert.That(bytes, Is.EquivalentTo(fibonacci));
        }

        [Test]
        public void HexToBytes_Given0xPrependedHexString_ReturnsByteArray()
        {
            var bytes = sut.HexToBytes(fibonacciPrepended);

            Assert.That(bytes, Is.EquivalentTo(fibonacci));
        }

        [Test]
        public void HexToBytes_GivenUpperCaseHexString_ReturnsByteArray()
        {
            var bytes = sut.HexToBytes(fibonacciPrepended.ToUpperInvariant());

            Assert.That(bytes, Is.EquivalentTo(fibonacci));
        }
    }
}
